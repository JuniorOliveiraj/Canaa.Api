using Canaa.DataContracts.AzureTTS;
using Canaa.Infra.ExternalServices.Azure.TTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Canaa.Infra.ExternalServices.Azure.TTS.AzureTextToSpeech;

namespace Canaa.Infra.ExternalServices.Azure
{
    public static class AzureCreate
    {
        public static async Task<situacaoCriacao> GeraraAudio(TtsCreateDataObject create)
        {
            return await AzureTextToSpeech.GenerateSpeechAsync(create.Texto, create.Caminho, AzureTTSVoice.ptBR_AntonioNeural);
        }
    }
}
