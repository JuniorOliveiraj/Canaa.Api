using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.AzureTTS
{
    public class TtsCreateDataObject
    {
        public string Texto { get; set; }
        public string Caminho { get; set; }
        public AzureTTSVoice Voz { get; set; } = AzureTTSVoice.ptBR_AntonioNeural;
        public Action<string> OnProgress { get; set; }
        public TimeSpan? Timeout { get; set; } = TimeSpan.FromMinutes(5);
    }
}
