using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class TikTokUploader
{
    private readonly HttpClient _httpClient;
    private readonly string _accessToken = "act.example12345Example12345Example"; // seu token real aqui

    public TikTokUploader()
    {
        _httpClient = new HttpClient();
    }

    public async Task<(string uploadId, string uploadToken)?> InitUploadAsync(long videoSize, int chunkSize, int totalChunkCount)
    {
        var url = "https://open.tiktokapis.com/v2/post/publish/inbox/video/init/";

        var payload = new
        {
            source_info = new
            {
                source = "FILE_UPLOAD",
                video_size = videoSize,
                chunk_size = chunkSize,
                total_chunk_count = totalChunkCount
            }
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

        var response = await _httpClient.PostAsync(url, content);

        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(responseJson);
            var data = document.RootElement.GetProperty("data");

            string uploadId = data.GetProperty("upload_id").GetString()!;
            string uploadToken = data.GetProperty("upload_token").GetString()!;

            return (uploadId, uploadToken);
        }

        Console.WriteLine("Erro: " + await response.Content.ReadAsStringAsync());
        return null;
    }

    public async Task<bool> UploadVideoChunkAsync(string uploadId, string uploadToken, string filePath)
    {
        var url = $"https://open-upload.tiktokapis.com/video/?upload_id={uploadId}&upload_token={uploadToken}";

        byte[] videoBytes = await File.ReadAllBytesAsync(filePath);
        long totalSize = videoBytes.Length;

        var request = new HttpRequestMessage(HttpMethod.Put, url)
        {
            Content = new ByteArrayContent(videoBytes)
        };

        request.Content.Headers.ContentType = new MediaTypeHeaderValue("video/mp4");
        request.Content.Headers.ContentLength = totalSize;
        request.Headers.Add("Content-Range", $"bytes 0-{totalSize - 1}/{totalSize}");

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Vídeo enviado com sucesso.");
            return true;
        }

        Console.WriteLine("Erro: " + await response.Content.ReadAsStringAsync());
        return false;
    }
}
