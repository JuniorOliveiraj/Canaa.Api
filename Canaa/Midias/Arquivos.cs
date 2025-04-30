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
    }
}
