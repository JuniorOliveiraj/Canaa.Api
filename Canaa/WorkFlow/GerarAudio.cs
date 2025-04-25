using Canaa.Infra.ExternalServices.Azure;
using Canaa.Infra.ExternalServices.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.WorkFlow
{
    [ApiController]
    [Route("v1/[controller]")]
    public class GerarAudio: ControllerBase
    {

        [AllowAnonymous]
        [HttpGet("Gerar")]
        public async  Task<IActionResult> Gerar()
        {
            var resultado = await AzureCreate.GeraraAudio();

            return Ok(new
            {
                Ok = true,
                message = resultado
            });
        }
    }
}
