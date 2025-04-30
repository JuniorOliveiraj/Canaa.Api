using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Auth;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.FN.BusinessComponents.Video.CriarVideos;
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
    public class Gerar : ControllerBase
    {


        [AllowAnonymous]
        [HttpGet("video/youtube/baixar")]
        public async Task<IActionResult> BaixarVideo(string link = "https://www.youtube.com/watch?v=hZTumVn372c&ab_channel=TalkFlow")
        {
            string _guid = Guid.NewGuid().ToString();
            var videoPath = await YoutubeDownloader.DownloadVideoAsync(link, ApiLocation.DiretorioTemporario("Youtube"), _guid);
            byte[] videoBytes = System.IO.File.ReadAllBytes(videoPath);
            var base64String = Convert.ToBase64String(videoBytes);
            return Ok(new
            {
                Ok = true,
                videoPath,
                base64String
            });
        }
        [AllowAnonymous]
        [HttpGet("video/youtube/baixar-legenda")]
        public async Task<IActionResult> BaixarVideoLegenda(string link = "https://www.youtube.com/watch?v=hZTumVn372c&ab_channel=TalkFlow")
        {
            string tempDirectory = ApiLocation.DiretorioTemporario("Youtube");
            var videoPath = await YoutubeDownloader.DownloadCaptionsAsync(link, tempDirectory);

            return Ok(new
            {
                Ok = true,
                videoPath,
            });
        }
        [AllowAnonymous]
        [HttpGet("video/Vertica-gerar-compleo")]
        public async Task<IActionResult> CriarVideoCompleto(string videoTop, string videoButton)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.EmpilharVideosCompletoAsync(videoTop, videoButton);

            return Ok(Result);
        }

        [AllowAnonymous]
        [HttpGet("video/Vertica-gerar")]
        public async Task<IActionResult> CriarVideoVertical(string videoTop, string videoButton)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.EmpilharVideosAsync(videoTop, videoButton);
            return Ok(Result);
        }

        [AllowAnonymous]
        [HttpGet("video/Vertica-gerar-legendado")]
        public async Task<IActionResult> CriarVideoLegendar(string videoTop, string videoButton)
        {
            var VideosComponent = BusinessComponent.CreateInstance<IVideosVertical>();
            var Result = await VideosComponent.EmpilharVideosAsync(videoTop, videoButton);
            return Ok(Result);
        }



 
    }
}
