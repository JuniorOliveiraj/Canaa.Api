using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Videos.Streams;
using YoutubeExplode;

namespace Canaa.Infra.ExternalServices.Youtube
{
    public static class YoutubeDownloader
    {
        public static async Task DownloadAsync(string url, string outputDirectory)
        {
            var youtube = new YoutubeClient();

            var video = await youtube.Videos.GetAsync(url);
            var title = SanitizeFileName(video.Title);

            var streamManifest = await youtube.Videos.Streams.GetManifestAsync(video.Id);
            var stream = streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();

            if (stream is null)
            {
                Console.WriteLine($"⚠️ Stream não encontrado para: {video.Title}");
                return;
            }

            Directory.CreateDirectory(outputDirectory);
            var filePath = Path.Combine(outputDirectory, $"{title}.{stream.Container.Name}");

            await youtube.Videos.Streams.DownloadAsync(stream, filePath);
            Console.WriteLine($"✅ Download concluído: {filePath}");
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
