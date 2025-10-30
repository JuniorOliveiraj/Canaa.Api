using Canaa.AppHost.utils;
using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Midia.Video.Youtube;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Youtube;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Comunicacao.Youtube
{
    [ApiController]
    [Route("v1/[controller]")]
    public class YoutubeController : ControllerBase
    {

        [AllowAnonymous]
        [HttpPost("/baixar/video")]
        public async Task<IActionResult> BaixarVideo(string link)
        {
            var youtubeComponent = BusinessComponent.CreateInstance<IYoutubeComponent>();
            var response = await youtubeComponent.BaixarVideo(link);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("/baixar/legenda")]
        public async Task<IActionResult> BaixarVideoLegenda(string link)
        {
            var youtubeComponent = BusinessComponent.CreateInstance<IYoutubeComponent>();
            var response = await youtubeComponent.BaixarLegendasYoutube(link);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("/baixar/Audio")]
        public async Task<IActionResult> BaixarVideoAudio(string link)
        {
            var youtubeComponent = BusinessComponent.CreateInstance<IYoutubeComponent>();
            var response = await youtubeComponent.BaixarAudioYoutube(link);

            return Ok(response);
        }
    }
}
