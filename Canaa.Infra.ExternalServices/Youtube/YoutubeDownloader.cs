using Canaa.Infra.ExternalServices.Utils;
using Canaa.Infra.ExternalServices.Videos;
using Canaa.Infra.ExternalServices.Videos.Legenda;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;


namespace Canaa.Infra.ExternalServices.Youtube
{
    public static class YoutubeDownloader
    {
        /// <summary>
        /// Baixa um vídeo do YouTube (vídeo + áudio muxados) na melhor qualidade e retorna o caminho do arquivo.
        /// </summary>
        /// <param name="url">URL completa ou ID do vídeo.</param>
        /// <param name="outputDirectory">Pasta onde o arquivo será salvo.</param>
        /// <returns>O caminho completo do arquivo .mp4 baixado.</returns>
        public static async Task<string> DownloadVideoAsync(
            string url,
            string outputDirectory,
            string processId,
            CancellationToken cancellationToken = default
            )
        {
            var youtube = new YoutubeClient();
            ProgressService.UpdateTaskStatus(processId, "Iniciando");

            // 1) Obter metadados do vídeo
            var video = await youtube.Videos.GetAsync(url, cancellationToken);
            var safeTitle = Sanitize(video.Title);
            Directory.CreateDirectory(outputDirectory);
            var outputPath = Path.Combine(outputDirectory, $"{safeTitle}.mp4");

            if (File.Exists(outputPath))
            {
                return outputPath;
            }

            // 2) Buscar streams disponíveis
            var manifest = await youtube.Videos.Streams.GetManifestAsync(url, cancellationToken);

            // 3) Tentar baixar um stream muxed (priorizando 1080p)
            var muxedStream = manifest
                .GetMuxedStreams()
                .OrderByDescending(s => s.VideoQuality)
                .FirstOrDefault();

            if (muxedStream != null)
            {
                try
                {
                    ProgressService.UpdateTaskStatus(processId, "Baixando (muxed)");
                    var progress = new Progress<double>(p =>
                        ProgressService.SetProgress(processId, (int)(p * 100)));

                    await youtube.Videos.Streams.DownloadAsync(
                        muxedStream,
                        outputPath,
                        progress,
                        cancellationToken
                    );

                    return outputPath;
                }
                catch (Exception ex)
                {
                    ProgressService.UpdateTaskError(processId, ex.Message);
                    return "Erro";
                }
            }

            // 4) Fallback: Baixar vídeo + áudio separadamente e usar FFmpeg
            ProgressService.UpdateTaskStatus(processId, "Buscando streams separados");

            var videoStream = manifest
                .GetVideoStreams()
                .OrderByDescending(s => s.VideoQuality)
                .FirstOrDefault()
                ?? throw new InvalidOperationException("Nenhum stream de vídeo encontrado.");

            var audioStream = manifest
                .GetAudioStreams()
                .OrderByDescending(s => s.Bitrate)
                .FirstOrDefault()
                ?? throw new InvalidOperationException("Nenhum stream de áudio encontrado.");

            // 5) Configurar FFmpeg
            var progressFFmpeg = new Progress<double>(p =>
                ProgressService.SetProgress(processId, (int)(p * 100)));

            var conversionRequest = new ConversionRequestBuilder(outputPath)
                .SetPreset(ConversionPreset.Fast)
                .SetFFmpegPath("ffmpeg") // Certifique-se de que o FFmpeg está no PATH
                .Build();
            var streams = new IStreamInfo[] { videoStream, audioStream };  // Tipo explícito

            ProgressService.UpdateTaskStatus(processId, "Mesclando com FFmpeg");
            await youtube.Videos.DownloadAsync(
                streams,  // Agora com tipo definido
                conversionRequest,
                progressFFmpeg,
                cancellationToken
            );
            return outputPath;
        }



        public static async Task<string> DownloadAudioAsync(string url, string outputDirectory, string processId)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var title = Sanitize(video.Title);

            var manifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var audio = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            if (audio == null)
            {
                ProgressService.UpdateTaskError(processId, "❌ Nenhum stream de áudio disponível.");
                return "❌ Nenhum stream de áudio disponível.";
            }

            var progressFFmpeg = new Progress<double>(p =>
                    ProgressService.SetProgress(processId, (int)(p * 100)));

            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"{title}.{audio.Container.Name}");

            ProgressService.UpdateTaskStatus(processId, "Baixando áudio");
            await youtube.Videos.Streams.DownloadAsync(audio, path, progressFFmpeg);

            return path;
        }

        public static async Task<string> DownloadCaptionsAsync(string url, string outputDirectory, string language = "pt")
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var title = Sanitize(video.Title);

            var captionManifest = await youtube.Videos.ClosedCaptions.GetManifestAsync(video.Id);
            var track = captionManifest.GetByLanguage(language);

            if (track == null)
                return $"Erro";

            var captions = await youtube.Videos.ClosedCaptions.GetAsync(track);
            var captionText = string.Join(Environment.NewLine,
                captions.Captions.Select(c => $"{c.Offset}: {c.Text}"));

            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"{title}_captions_{language}.txt");
            await File.WriteAllTextAsync(path, captionText);

            var convert = LegendaConverter.ConverterTxtParaSrt(path);

            return convert;
        }


        public static async Task<string> DownloadVideoOnlyAsync(string url, string outputDirectory)
        {
            var youtube = new YoutubeClient();
            var video = await youtube.Videos.GetAsync(url);
            var title = Sanitize(video.Title);

            var manifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);

            // Pega o melhor stream de vídeo
            var videoStream = manifest
                .GetVideoOnlyStreams()
                .OrderByDescending(s => s.VideoQuality.MaxHeight)
                .FirstOrDefault();

            // Pega o melhor stream de áudio
            var audioStream = manifest
                .GetAudioOnlyStreams()
                .OrderByDescending(s => s.Bitrate)
                .FirstOrDefault();

            // Verifica se encontrou os streams
            if (videoStream == null || audioStream == null)
                throw new Exception("❌ Não foi possível encontrar um stream de vídeo ou áudio.");

            // Cria diretório de saída, se necessário
            Directory.CreateDirectory(outputDirectory);

            string videoPath = Path.Combine(outputDirectory, $"{title}_video.{videoStream.Container.Name}");
            string audioPath = Path.Combine(outputDirectory, $"{title}_audio.{audioStream.Container.Name}");

            // Baixar o vídeo e áudio separadamente
            await youtube.Videos.Streams.DownloadAsync(videoStream, videoPath);
            await youtube.Videos.Streams.DownloadAsync(audioStream, audioPath);

            // Verifique se os arquivos de vídeo e áudio foram baixados corretamente
            if (!File.Exists(videoPath))
            {
                throw new Exception($"❌ O vídeo não foi baixado corretamente. Caminho: {videoPath}");
            }

            if (!File.Exists(audioPath))
            {
                throw new Exception($"❌ O áudio não foi baixado corretamente. Caminho: {audioPath}");
            }

            // Juntar vídeo e áudio
            // var mergedVideoPath = await MargeVideos.MergeAudioAndVideoAsync(videoPath, audioPath, outputDirectory, true);

            return "mergedVideoPath";
        }


        private static string Sanitize(string input)
        {
            var invalid = Path.GetInvalidFileNameChars();
            foreach (var c in invalid)
                input = input.Replace(c, '_');
            return input;
        }
    }
}
