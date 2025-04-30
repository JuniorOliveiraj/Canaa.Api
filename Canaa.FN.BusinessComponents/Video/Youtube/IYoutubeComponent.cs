using Canaa.DataContracts.Videos;

namespace Canaa.FN.BusinessComponents.Video.Youtube
{
    public interface IYoutubeComponent
    {
        StartProcessResponse BaixarVideo(string link);
    }
}