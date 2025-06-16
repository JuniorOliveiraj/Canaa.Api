using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;
using Microsoft.AspNetCore.Http;

namespace Canaa.FN.BusinessComponents.Midia.Video.Youtube
{
    public interface IYoutubeComponent
    {
        Task<ResponseDataContrac> BaixarVideo(string link);
        Task<ResponseDataContrac> BaixarLegendasYoutube(string link);
        Task<ResponseDataContrac> BaixarAudioYoutube(string link);
    }
}