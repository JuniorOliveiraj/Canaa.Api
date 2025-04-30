using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Videos.Youtube
{
    public class DownloadTask
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
        public int Progress { get; set; } = 0;
        public string? FilePath { get; set; }
        public string? Error { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
