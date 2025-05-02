using Canaa.DataContracts.Videos;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Videos
{
    public class LegendarVideo
    {
        public string Start(LegendarVideosDataObject video)
        {
            string log = string.Empty;
            if (!File.Exists(video.Video) || !File.Exists(video.Legendas))
                return "Arquivo de vídeo ou legenda não encontrado!";

            // Escapa caminho da legenda
            string subsPath = Path.GetFullPath(video.Legendas)
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
            string arguments = $"-y -i \"{video.Video}\" -vf \"{filtro}\" -c:a copy \"{video.FinalComLegenda}\"";

            //string arguments = $"-y -i \"{video}\" -vf \"{filtro}\" -c:a copy \"{video.FinalComLegenda}\"";

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
                            video.OnProgress?.Invoke(e.Data);
                            Console.WriteLine($"[ffmpeg] {e.Data}");
                            log += $"[ffmpeg] {e.Data}" + Environment.NewLine;
                        }
                    };

                    process.Start();
                    process.BeginErrorReadLine();

                    // aguarda saída, respeitando timeout
                    bool finished = process.WaitForExit(video.Timeout?.Milliseconds ?? 10 * 60 * 1000);
                    if (!finished)
                    {
                        try { process.Kill(); } catch { }
                        return "Erro: tempo de execução excedido.";
                    }

                    if (process.ExitCode != 0)
                    {
                        return $"Erro FFmpeg (ExitCode={process.ExitCode}). Log: {log}";
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
}
