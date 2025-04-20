using Microsoft.AspNetCore.Mvc;

namespace SuaAppNamespace.Controllers // substitua pelo namespace do seu projeto
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet("saudacao")]
        public IActionResult Saudacao()
        {
            return Ok("Olá do controller! 👋");
        }

        [HttpGet("hora")]
        public IActionResult HoraAtual()
        {
            return Ok($"Agora são {DateTime.Now:HH:mm:ss}");
        }
    }
}
