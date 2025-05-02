using Canaa.DataContracts.Videos;
using Canaa.Infra.ExternalServices.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg.Downloader;
using Xabe.FFmpeg;

namespace Canaa.Infra.ExternalServices.Videos
{
    public class EmpilharVideos
    {
        public async Task<string> Start(EmpilharVideosDataObject data)
        {
            await EnsureFFmpegDownloadedAsync();

            string sanitizedTop = SanitizeFilename(Path.GetFileNameWithoutExtension(data.VideoTop));
            string sanitizedBottom = SanitizeFilename(Path.GetFileNameWithoutExtension(data.VideoBottom));
            string outputPath = Path.Combine(data.OutputDirectory, $"stacked_{sanitizedTop}_{sanitizedBottom}.mp4");

            string filter = CreateStackFilter(data.TargetWidth);

            try
            {
                if (data.ProcessId != null)
                    ProgressService.UpdateTaskStatus(data.ProcessId, "Empilhando Videos");

                var progress = new Progress<double>(p =>
                {
                    int percent = (int)(p * 100);
                    if (data.ProcessId != null)
                        ProgressService.SetProgress(data.ProcessId, (int)(p * 100));
                });

                await ConvertVideosAsync(data.VideoTop, data.VideoBottom, outputPath, filter, progress);
                FFMpegKiller.MatarFFMPEG();

                ProgressService.UpdateTaskStatus(data.ProcessId, "Empilhando Videos: Concluido");
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
    }
}
