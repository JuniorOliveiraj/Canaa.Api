using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Videos
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TarefasCategorias
    {
        [JsonPropertyName("Baixar Video")]
        BaixarVideoYoutube,
        [JsonPropertyName("Baixar legendas")]
        BaixarLegenda,
        [JsonPropertyName("Baixar Audio")]
        BaixarAudio,
        [JsonPropertyName("Empilhar Videos")]   
        EmpilharVideos,
        [JsonPropertyName("Legendar Videos")]
        LegendarVideos,
        [JsonPropertyName("Criar video completo")]
        CriarVideoCompleto,

    }
}
