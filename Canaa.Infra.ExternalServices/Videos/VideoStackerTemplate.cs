using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using Canaa.Infra.ExternalServices.Videos;
using Canaa.Infra.ExternalServices.Utils;

public static class VideoStackerTemplate
{
    /// <summary>
    /// Empilha dois vídeos verticalmente em proporção 9:16, sem espaços sobrando, e usa só o áudio do primeiro.
    /// </summary>
    public static async Task<string> StackVerticallyAsync(
        string videoTop,
        string videoBottom,
        string outputDirectory,
        int targetWidth = 1080, 
        string processId = null
        )
    {
        await EnsureFFmpegDownloadedAsync();

        string sanitizedTop = SanitizeFilename(Path.GetFileNameWithoutExtension(videoTop));
        string sanitizedBottom = SanitizeFilename(Path.GetFileNameWithoutExtension(videoBottom));
        string outputPath = Path.Combine(outputDirectory, $"stacked_{sanitizedTop}_{sanitizedBottom}.mp4");

        string filter = CreateStackFilter(targetWidth);

        try
        {
            if (processId != null)
                ProgressService.UpdateTaskStatus(processId, "Empilhando Videos");

            var progress = new Progress<double>(p =>
            {
                int percent = (int)(p * 100);
                if (processId != null)
                    ProgressService.SetProgress(processId, (int)(p * 100));
            });

            await ConvertVideosAsync(videoTop, videoBottom, outputPath, filter, progress);
            FFMpegKiller.MatarFFMPEG();

            ProgressService.UpdateTaskStatus(processId, "Empilhando Videos: Concluido");
            return outputPath;
        }
        catch (Exception ex)
        {
            throw new Exception($"❌ Erro ao executar FFmpeg: {ex.Message}", ex);
        }
    }


     


    private static async Task EnsureFFmpegDownloadedAsync()
    {
        string ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg");
        if (!Directory.Exists(ffmpegPath))
        {
            Directory.CreateDirectory(ffmpegPath);
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegPath);
        }
        FFmpeg.SetExecutablesPath(ffmpegPath);
    }

    private static string SanitizeFilename(string filename)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return string.Join("_", filename.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
    }

    private static string CreateStackFilter(int targetWidth)
    {
        // Calcula a altura final 9:16 e a metade para cada vídeo
        int targetHeight = targetWidth * 16 / 9;
        int halfHeight = targetHeight / 2;

        // Scale + crop para não deixar espaços vazios
        string topFilter = $"[0:v]" +
            $"scale=w={targetWidth}:h={halfHeight}:force_original_aspect_ratio=increase," +
            $"crop=w={targetWidth}:h={halfHeight}[top];";

        string bottomFilter = $"[1:v]" +
            $"scale=w={targetWidth}:h={halfHeight}:force_original_aspect_ratio=increase," +
            $"crop=w={targetWidth}:h={halfHeight}[bottom];";

        return topFilter + bottomFilter + "[top][bottom]vstack=inputs=2[v]";
    }

    private static async Task ConvertVideosAsync(
        string videoTop,
        string videoBottom,
        string outputPath,
        string filter,
        IProgress<double> progresso = null
        )
    {
        var conversion = FFmpeg.Conversions.New()
            .SetOverwriteOutput(true)
            // Faz o vídeo de baixo repetir infinitamente (-stream_loop -1 antes do segundo input)
            .AddParameter($"-i \"{videoTop}\" -stream_loop -1 -i \"{videoBottom}\"")
            .AddParameter($"-filter_complex \"{filter}\" -shortest -map \"[v]\" -map 0:a")
            .AddParameter("-c:v libx264 -c:a copy -preset veryfast -movflags +faststart")
            .SetOutput(outputPath);

        conversion.OnProgress += (sender, args) =>
        {
            double progress = args.Duration.TotalSeconds > 0
                ? args.Duration.TotalSeconds / args.TotalLength.TotalSeconds
                : 0;
            progresso?.Report(progress);
        };

        await conversion.Start();
    }

    public static string AdicionarLegendasAoVideo(
     string video,
     string legendas,
     string finalComLegenda,
     Action<string> onProgress = null,     // callback opcional
     TimeSpan? timeout = null              // timeout opcional
 )
    {
        if (!File.Exists(video) || !File.Exists(legendas))
            return "Arquivo de vídeo ou legenda não encontrado!";

        // Escapa caminho da legenda
        string subsPath = Path.GetFullPath(legendas)
                             .Replace("\\", "/")
                             .Replace(":", "\\:");

        string estilo = "FontName=Inter,Fontsize=11," +
                        "PrimaryColour=&H0000FFFF," +   
                        "Outline=1," +
                        "OutlineColour=&H00000000," +           
                        "Shadow=0," +
                        "BackColour=&H00000000," +              
                        "Bold=1," +
                        "Alignment=10," +                       
                        "MarginV=150";                           


        string filtro = $"subtitles='{subsPath}':charenc=UTF-8:force_style='{estilo}'";
        string arguments = $"-y -i \"{video}\" -vf \"{filtro}\" -c:a copy \"{finalComLegenda}\"";

        try
        {
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = arguments,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Para disparar evento Exited se quiser
                process.EnableRaisingEvents = true;

                // Captura log de progresso
                process.ErrorDataReceived += (s, e) =>
                {
                    if (!string.IsNullOrWhiteSpace(e.Data))
                    {
                        // envia para o callback (caso não seja null) e/ou imprime no console
                        onProgress?.Invoke(e.Data);
                        Console.WriteLine($"[ffmpeg] {e.Data}");
                    }
                };

                process.Start();
                process.BeginErrorReadLine();

                // aguarda saída, respeitando timeout
                bool finished = process.WaitForExit(timeout?.Milliseconds ?? 10 * 60 * 1000);
                if (!finished)
                {
                    try { process.Kill(); } catch { }
                    return "Erro: tempo de execução excedido.";
                }

                if (process.ExitCode != 0)
                {
                    return $"Erro FFmpeg (ExitCode={process.ExitCode}). Veja o log acima para detalhes.";
                }
            }

            return "Deu Boa";
        }
        catch (Exception ex)
        {
            // aqui você captura qualquer outra falha (ex: File I/O, permissão, etc)
            return $"Erro inesperado: {ex.Message}";
        }
    }


}
