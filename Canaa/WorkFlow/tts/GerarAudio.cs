using Canaa.AppHost.utils;
using Canaa.DataContracts.AzureTTS;
using Canaa.FN.BusinessComponents.Midia.Audio;
using Canaa.FN.BusinessComponents.Response;
using Canaa.Infra.ExternalServices.Azure;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.WorkFlow.tts
{
    [ApiController]
    [Route("v1/[controller]")]
    public class GerarAudio : ControllerBase
    {

        [HttpPost("Gerar")]
        public async Task<IActionResult> GerarAudioAsync(string imput, AzureTTSVoice voz)
        {
            TtsCreateDataObject data = new TtsCreateDataObject
            {
                Texto = imput,
                Voz = voz
            };
            var criarAudioComponent = BusinessComponent.CreateInstance<IGerarAudioComTts>();
            ResponseDataContrac responseDataContrac = await criarAudioComponent.GerarAudioAsync(data);
            if (!responseDataContrac.success)
            {
                return BadRequest(responseDataContrac.message);
            }

            return Ok(responseDataContrac);
        }
       }
}
