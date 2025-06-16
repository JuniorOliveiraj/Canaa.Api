using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Canaa.AppHost.utils;
using Canaa.DataContracts.AzureTTS;
using Microsoft.Extensions.Configuration;

namespace Canaa.Infra.ExternalServices.Azure.TTS
{
    public static class AzureTextToSpeech
    {

        public static async Task<situacaoCriacao> GenerateSpeechAsync(string text, string outputPath, AzureTTSVoice voice)
        {
            string voiceName = voice.ToString().Replace("ptBR_", "pt-BR-");
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    return Error("❌ Texto de entrada vazio.");

                var config = Config.Build();
                var subscriptionKey = config["Azure:chave"];
                var region = config["Azure:region"];

                if (string.IsNullOrEmpty(subscriptionKey) || string.IsNullOrEmpty(region))
                    return Error("❌ Configuração inválida: verifique se Azure:chave e Azure:region estão definidos.");

                string url = $"https://{region}.tts.speech.microsoft.com/cognitiveservices/v1";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                    client.DefaultRequestHeaders.Add("User-Agent", "CanaaTTSApp");

                    // Limpa o XML (sem espaços e quebras desnecessárias)
                    string ssml = $@"<speak version='1.0' xml:lang='pt-BR'><voice xml:lang='pt-BR' xml:gender='Female' name='{voiceName}'>{System.Security.SecurityElement.Escape(text)}</voice></speak>";

                    var content = new StringContent(ssml, Encoding.UTF8, "application/ssml+xml");

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url)
                    {
                        Content = content
                    };

                    request.Headers.Add("X-Microsoft-OutputFormat", "audio-24khz-160kbitrate-mono-mp3");

                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
                        await File.WriteAllBytesAsync(outputPath, audioBytes);

                        return Sucesso(outputPath);
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        string mensage = $"❌ Falha na solicitação Azure TTS\n" +
                               $"📡 Status: {response.StatusCode}\n" +
                               $"📄 Detalhes: {errorContent}";

                        return Error(mensage);
                    }
                }
            }
            catch (Exception ex)
            {
                return Error($"❌ Erro inesperado ao gerar fala:\n🧨 {ex.Message}");
            }
        }
        private static situacaoCriacao Error(string mensagem)
        {
            situacaoCriacao situacaoCriacao = new situacaoCriacao();
            situacaoCriacao.sucesso = false;
            situacaoCriacao.mensagem = mensagem;
            return situacaoCriacao ;    
        }
        private static situacaoCriacao Sucesso (string mensagem)
        {
            situacaoCriacao situacaoCriacao = new situacaoCriacao();
            situacaoCriacao.sucesso = true;
            situacaoCriacao.mensagem = mensagem;
            return situacaoCriacao;
        }
        
      
    }
}
