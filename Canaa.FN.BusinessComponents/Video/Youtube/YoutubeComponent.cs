using Canaa.DataContracts.Videos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Video.Youtube
{
    public class YoutubeComponent : IYoutubeComponent
    {
        public StartProcessResponse BaixarVideo(string link)
        {
             
            return new StartProcessResponse { ProcessId = "1" };
        }
    }
}
