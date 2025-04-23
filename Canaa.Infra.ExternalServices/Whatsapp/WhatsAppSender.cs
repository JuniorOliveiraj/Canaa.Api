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

    public static class WhatsAppSender
    {
        public static async Task<string> Mensage(SendTextMessageDataObjec request)
        {
            return await EnviarMensagem.SendMensage(request);
        }

        public static async Task<string> Imagem(SendMidiaDataObject request)
        {
            return await EnviarImagemUrl.SendImagem(request);
        }
    }
}

