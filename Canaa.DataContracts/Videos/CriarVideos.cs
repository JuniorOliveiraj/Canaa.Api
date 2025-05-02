using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Videos
{
    public class LegendarVideosDataObject
    {
        public string Video { get; set; }
        public string Legendas { get; set; }
        public string FinalComLegenda { get; set; }
        public Action<string> OnProgress { get; set; }
        public TimeSpan? Timeout { get; set; }
    }
    public class EmpilharVideosDataObject
    {
        public string VideoTop { get; set; }
        public string VideoBottom { get; set; }
        public string OutputDirectory { get; set; }
        public int TargetWidth { get; set; } = 1080;
        public string ProcessId { get; set; }
    }
}
