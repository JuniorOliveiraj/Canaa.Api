using Canaa.DataContracts.Whatsapp;
using Canaa.Infra.ExternalServices.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Canaa.WhatsApp.WhatsappController;

namespace Canaa.Midias.Postagem
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PostarTiktocController : ControllerBase
    {
        [Authorize]
        [HttpPost("post")]
        public async Task<IActionResult> PostNow(SendMensage request)
        {
            var uploader = new TikTokUploader();
            string videoPath = @"C:\Downloads\Vídeo sem título ‐ Feito com o Clipchamp (1).mp4";

            FileInfo fileInfo = new FileInfo(videoPath);
            long videoSize = fileInfo.Length;
            int chunkSize = (int)videoSize; // se for enviar de uma vez só
            int totalChunks = 1;

            var init = await uploader.InitUploadAsync(videoSize, chunkSize, totalChunks);

            if (init != null)
            {
                var (uploadId, uploadToken) = init.Value;
                await uploader.UploadVideoChunkAsync(uploadId, uploadToken, videoPath);
                return Ok("Postado");
            }
            else
            {
                Console.WriteLine("Falha ao iniciar upload.");
                return Ok("Falha ao iniciar upload.");
            }
            

        }
    }
}
