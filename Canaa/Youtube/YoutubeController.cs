using Canaa.AppHost.utils;
using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Video.Youtube;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Youtube
{
    [ApiController]
    [Route("v1/[controller]")]
    public class YoutubeController : ControllerBase
    {
        [HttpPost("start")]
        public ActionResult<StartProcessResponse> StartProcess([FromBody] string link)
        {
            var youtubeComponent = BusinessComponent.CreateInstance<IYoutubeComponent>();
            var response = youtubeComponent.BaixarVideo(link);
            return Ok(response);
        }
    }
}
