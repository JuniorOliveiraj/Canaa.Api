using Canaa.AppHost.utils;
using Canaa.DataContracts.Whatsapp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Whatsapp
{
    public static class EnviarImagemUrl
    {
        public static async Task<string> SendImagem(SendMidiaDataObject request)
        {
            if (request == null)
                return "Erro: requisição nula.";

            var config = Config.Build(); // <-- usando padrão tipo o seu `Query` para pegar config
            var baseUrl = config["WhatsAppApi:BaseUrl"];
            var apiKey = config["WhatsAppApi:ApiKey"];

            var dtoMessage = new SendMidiaDataObject
            {
                number = request.number ?? string.Empty,
                options = new OptionsDataObjec
                {
                    delay = request.options?.delay == 0 ? 1200 : request.options?.delay ?? 1200,
                    presence = "composing",
                    linkPreview = request.options?.linkPreview ?? false
                },
               mediaMessage = new MediaMessage
               {
                   mediatype = request.mediaMessage.mediatype,
                   caption = request.mediaMessage?.caption ?? string.Empty,
                   media = request.mediaMessage?.media ?? string.Empty
               }
            };

            var json = JsonSerializer.Serialize(dtoMessage);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("apikey", apiKey);

            var url = $"{baseUrl}/message/sendMedia/Bot";

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
                return "Mensagem enviada com sucesso!";
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return $"Erro: {response.StatusCode} - {error}";
            }
        }
    }
}
