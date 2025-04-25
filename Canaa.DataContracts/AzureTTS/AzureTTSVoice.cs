using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.DataContracts.AzureTTS
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AzureTTSVoice
    {
        ptBR_AntonioNeural,
        ptBR_BrendaNeural,
        ptBR_DanielNeural,
        ptBR_DonatoNeural,
        ptBR_ElzaNeural,
        ptBR_FabioNeural,
        ptBR_FranciscaNeural,
        ptBR_GiovannaNeural,
        ptBR_HumbertoNeural,
        ptBR_JulioNeural,
        ptBR_LeilaNeural,
        ptBR_LeticiaNeural,
        ptBR_MacerioMultilingualNeural,
        ptBR_ManuelaNeural,
        ptBR_NicolauNeural,
        ptBR_ThalitaNeural,
        ptBR_ThalitaMultilingualNeural,
        ptBR_ValerioNeural,
        ptBR_YaraNeural
    }
}
