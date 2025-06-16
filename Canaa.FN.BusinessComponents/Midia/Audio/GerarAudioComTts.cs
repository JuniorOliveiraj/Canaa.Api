using Canaa.DataContracts.AzureTTS;
using Canaa.FN.BusinessComponents.Response;
using Canaa.Infra.ExternalServices.Azure;
using Canaa.Infra.ExternalServices.Utils.Genericos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Midia.Audio
{
    public class GerarAudioComTts : IGerarAudioComTts
    {
        // Implementação do método para gerar áudio com TTS (Text-to-Speech)
        public async Task<ResponseDataContrac> GerarAudioAsync(TtsCreateDataObject data)
        {
            if (String.IsNullOrEmpty(data.Texto))
                return Error("O texto para o audio ser gerado deve ser enviado");

            TtsCreateDataObject criar = new TtsCreateDataObject();
            string nomeArquivo = DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + Guid.NewGuid().ToString() + ".mp3";
            string pasta = CriarPastas.CriarPasta("tts");
            string diretorio = Path.Combine(pasta, nomeArquivo);

            criar.Caminho = diretorio;
            criar.Texto = data.Texto;
            criar.Voz = data.Voz;


            situacaoCriacao audioPath = await AzureCreate.GeraraAudio(criar);
            if (!audioPath.sucesso)
            {
                return Error(audioPath.mensagem);
            }


            return Sucess(audioPath.mensagem);
        }



        private ResponseDataContrac Error(string erro)
        {
            ResponseDataContrac responseDataContrac = new ResponseDataContrac();
            responseDataContrac.success = false;
            responseDataContrac.message = erro;
            responseDataContrac.error = erro;
            responseDataContrac.status = "500";
            return responseDataContrac;
        }

        private ResponseDataContrac Sucess(string arquivo)
        {
            ResponseDataContrac responseDataContrac = new ResponseDataContrac();
            responseDataContrac.success = true;
            responseDataContrac.message = "Áudio gerado com sucesso.";
            responseDataContrac.data = new { arquivo = arquivo };
            responseDataContrac.status = "200";
            return responseDataContrac;
        }
    }
}
