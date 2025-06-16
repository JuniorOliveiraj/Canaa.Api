using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Auth;
using Canaa.FN.BusinessComponents.Midia.Video.CriarVideos;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Azure;
using Canaa.Infra.ExternalServices.Videos;
using Canaa.Infra.ExternalServices.Youtube;
using CliWrap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Canaa.WorkFlow
{
    [ApiController]
    [Route("v1/[controller]")]
    public class GerarVideoController : ControllerBase
    {

        [Authorize]
        [HttpPost("video/Empilhar-com-legendas")]
        public async Task<IActionResult> CriarVideoCompleto(string videoTop, string videoButton)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.EmpilharVideosLegendar(videoTop, videoButton);

            return Ok(Result);
        }

        [Authorize]
        [HttpPost("video/Empilhar")]
        public async Task<IActionResult> CriarVideoVertical(string videoTop, string videoButton)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.EmpilharVideo(videoTop, videoButton);
            return Ok(Result);
        }

        [Authorize]
        [HttpPost("video/Legendar")]
        public async Task<IActionResult> CriarVideoLegendar(string pathVideo, string pathLegendas)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.AdicionarLegendasAoVideo(pathVideo, pathLegendas);
            return Ok(Result);
        } 
    }
}
