using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;

namespace Canaa.FN.BusinessComponents.Video.CriarVideos
{
    public interface IVideosVertical
    {
        Task<ResponseDataContrac> EmpilharVideosCompletoAsync(string linkVideoTop, string linkVideoButton);
        Task<ResponseDataContrac> EmpilharVideosAsync(string linkVideoTop, string linkVideoButton);
    }
}