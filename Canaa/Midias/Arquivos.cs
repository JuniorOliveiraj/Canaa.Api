using Microsoft.AspNetCore.Mvc;

namespace Canaa.Midias
{
    [ApiController]
    [Route("v1/[controller]")]
    public class Arquivos : ControllerBase
    {
        [HttpGet]
        [Route("stream")]
        public IActionResult GetVideo(string caminho)
        {               
            if (!System.IO.File.Exists(caminho))
                return NotFound();

            var stream = new FileStream(caminho, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "video/mp4", enableRangeProcessing: true);
        }

        [HttpGet]
        [Route("audio")]
        public IActionResult GetAudio(string caminho)
        {
            if (!System.IO.File.Exists(caminho))
                return NotFound();

            var stream = new FileStream(caminho, FileMode.Open, FileAccess.Read, FileShare.Read);
            return File(stream, "audio/mpeg", enableRangeProcessing: true);
        }

        [HttpGet]
        [Route("download")]
        public IActionResult DownloadArquivo(string caminho)
        {
            if (!System.IO.File.Exists(caminho))
                return NotFound();

            var fileName = Path.GetFileName(caminho);
            var fileBytes = System.IO.File.ReadAllBytes(caminho);

            return File(fileBytes, "application/octet-stream", fileName);
        }


    }
}
