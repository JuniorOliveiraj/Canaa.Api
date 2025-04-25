using Canaa.DataContracts.AzureTTS;
using Canaa.Infra.ExternalServices.Azure.TTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Azure
{
    public static class AzureCreate
    {
        public static async Task<string> GeraraAudio()
        {
            string texto = "Olá! Este é um teste de fala gerado com o serviço da Azure.";
            //string caminho = Path.Combine(Environment.CurrentDirectory, "fala.mp3");
            string caminho = @"D:\CANAA\fala.mp3";
            return await AzureTextToSpeech.GenerateSpeechAsync(texto, caminho, AzureTTSVoice.ptBR_AntonioNeural);

        }
    }
}
