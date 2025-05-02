using Canaa.DataContracts.Videos;
using Canaa.FN.BusinessComponents.Response;

namespace Canaa.FN.BusinessComponents.Video.CriarVideos
{
    public interface IVideosVertical
    {
        Task<ResponseDataContrac> EmpilharVideosLegendar(string linkVideoTop, string linkVideoButton);
        Task<ResponseDataContrac> EmpilharVideo(string linkVideoTop, string linkVideoButton);
        Task<ResponseDataContrac> AdicionarLegendasAoVideo(string pathVideoPrincipal, string pathLegendas);

    }
}