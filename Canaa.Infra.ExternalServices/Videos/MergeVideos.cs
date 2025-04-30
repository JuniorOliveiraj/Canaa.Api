using System.Diagnostics;
using System.Text;

namespace Canaa.Infra.ExternalServices.Videos
{
    public static class MergeVideos
    {
        public static async Task<string> MergeAudioAndVideoAsync(string videoPath, string audioPath, string outputDirectory, bool useGpu)
        {
            Directory.CreateDirectory(outputDirectory);

            var fileName = Path.GetFileNameWithoutExtension(videoPath);
            var mergedPath = Path.Combine(outputDirectory, $"{fileName}_merged.mp4");

            string audioCodec = GetBestAudioEncoder();
            bool requiresTranscoding = !await IsAudioCompatibleAsync(audioPath);
            string videoCodec = useGpu ? GetBestGpuEncoder() : "copy";

            string ffmpegArgs = BuildFfmpegArguments(videoPath, audioPath, mergedPath, videoCodec, audioCodec, requiresTranscoding);

            try
            {
                await RunFfmpegProcessAsync(ffmpegArgs);
                return $"✅ Merge concluído: {mergedPath}";
            }
            catch (Exception ex)
            {
                File.Delete(mergedPath);

                try
                {
                    string fallbackArgs = BuildFallbackArguments(videoPath, audioPath, mergedPath);
                    await RunFfmpegProcessAsync(fallbackArgs);
                    return $"⚠️ Merge feito via fallback: {mergedPath}";
                }
                catch (Exception fallbackEx)
                {
                    throw new Exception($"❌ Falha total na mesclagem: {fallbackEx.Message}", fallbackEx);
                }
            }
        }

        private static string GetBestAudioEncoder()
        {
            try
            {
                var output = RunSimpleProcess("ffmpeg", "-hide_banner -encoders");
                if (output.Contains("libfdk_aac")) return "libfdk_aac";
                if (output.Contains("aac")) return "aac";
            }
            catch
            {
                // Se falhar, usa copy
            }
            return "copy";
        }

        private static async Task<bool> IsAudioCompatibleAsync(string audioPath)
        {
            string ext = Path.GetExtension(audioPath).ToLower();
            return ext == ".aac" || ext == ".m4a";
        }

        private static string GetBestGpuEncoder()
        {
            try
            {
                var output = RunSimpleProcess("ffmpeg", "-hide_banner -encoders");

                if (output.Contains("h264_nvenc")) return "h264_nvenc"; // NVIDIA
                if (output.Contains("h264_amf")) return "h264_amf";     // AMD
                if (output.Contains("h264_qsv")) return "h264_qsv";     // Intel

                // Nenhum encontrado, usar CPU
                return "copy";
            }
            catch
            {
                return "copy";
            }
        }

        private static string BuildFfmpegArguments(string videoPath, string audioPath, string outputPath, string videoCodec, string audioCodec, bool needsTranscoding)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("-y -i \"{0}\" -i \"{1}\" ", videoPath, audioPath);
            builder.AppendFormat("-c:v {0} ", videoCodec);
            builder.AppendFormat("-c:a {0} ", needsTranscoding ? audioCodec : "copy");
            builder.Append("-ar 44100 -ac 2 -b:a 192k ");
            builder.Append("-movflags +faststart -shortest ");
            builder.Append("-map 0:v:0 -map 1:a:0 ");
            builder.Append("-max_interleave_delta 0 ");
            builder.AppendFormat("\"{0}\"", outputPath);

            return builder.ToString();
        }

        private static string BuildFallbackArguments(string videoPath, string audioPath, string outputPath)
        {
            return $"-y -i \"{videoPath}\" -i \"{audioPath}\" " +
                   "-c:v copy -c:a copy -map 0:v:0 -map 1:a:0 " +
                   $"\"{outputPath}\"";
        }

        private static string RunSimpleProcess(string fileName, string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private static async Task RunFfmpegProcessAsync(string arguments)
        {
            using var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = arguments,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();

            process.OutputDataReceived += (_, e) => { if (e.Data != null) outputBuilder.AppendLine(e.Data); };
            process.ErrorDataReceived += (_, e) => { if (e.Data != null) errorBuilder.AppendLine(e.Data); };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            await process.WaitForExitAsync().WaitAsync(TimeSpan.FromMinutes(10));

            if (process.ExitCode != 0)
            {
                throw new Exception($"FFmpeg falhou (ExitCode {process.ExitCode}):\n{errorBuilder}");
            }
        }
    }
}
