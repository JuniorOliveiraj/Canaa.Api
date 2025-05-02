using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Text;
using System.Diagnostics;
using Canaa.Infra.ExternalServices.Videos;
using Canaa.Infra.ExternalServices.Utils;
using Canaa.DataContracts.Videos;

public static class VideoStackerTemplate
{

    /// <summary>
    /// Empilha dois vídeos verticalmente em proporção 9:16, sem espaços sobrando, e usa só o áudio do primeiro.
    /// </summary>
    public static async Task<string> StackVerticallyAsync(EmpilharVideosDataObject data)
    {
        EmpilharVideos empilharVideos = new EmpilharVideos();
        return await empilharVideos.Start(data);
    }

    public static string AdicionarLegendasAoVideo(LegendarVideosDataObject start)
    { 
        LegendarVideo legendar = new LegendarVideo();         
        return legendar.Start(start);
    }


}
