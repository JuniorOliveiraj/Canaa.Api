using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Videos
{
    public class StartProcessResponse
    {
        public string ProcessId { get; set; } = string.Empty;
        public string Message { get; set; } = "Process started.";
    }

    public class ProgressResponse
    {
        public string ProcessId { get; set; } = string.Empty;
        public double Progress { get; set; }
        public string Status { get; set; } = "In Progress";
    }
}
