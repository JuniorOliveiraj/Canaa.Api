using Canaa.DataContracts.Whatsapp;
using Canaa.Infra.ExternalServices.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Canaa.Comunicacao.WhatsApp
{

    [ApiController]
    [Route("v1/[controller]")]
    public class WhatsappController : ControllerBase
    {

        [Authorize]
        [HttpPost("Send/mensage")]
        public async Task<IActionResult> Enviarmenagem(SendMensage request)
        {
            var mensagem = new SendTextMessageDataObjec
            {
                number = request.to,
                textMessage = new TextMessage
                {
                    text = request.textMessage
                }
            };

            var resultado = await WhatsAppSender.Mensage(mensagem);

            return Ok(new { resultado });
        }


        [AllowAnonymous]
        [HttpPost("Send/imagemUrl")]
        public async Task<IActionResult> EnviarImagemUrl(SendMensage request, string link)
        {

 
            byte[] imageBytes = System.IO.File.ReadAllBytes(link);
            var base64String = Convert.ToBase64String(imageBytes);
            var mensagem = new SendMidiaDataObject
            {
                number = request.to,
                instancia = request.instancia,
                mediaMessage = new MediaMessage
                {
                    mediatype = Mediatype.image,
                    caption = request.textMessage,
                    media = base64String //"https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png"
                }
            };

            var resultado = await WhatsAppSender.Imagem(mensagem);

            return Ok(new { resultado });
        }

        [AllowAnonymous]
        [HttpPost("Send/mensage-public")]
        public async Task<IActionResult> EnviarMenagemPublic(SendMensage request)
        {
            var mensagem = new SendTextMessageDataObjec
            {
                number = request.to,
                textMessage = new TextMessage
                {
                    text = request.textMessage
                },
                instancia = request.instancia ?? "Bot"

            };
            var resultado = await WhatsAppSender.Mensage(mensagem);
            return Ok(new { resultado });
        }


        public class SendMensage
        {
            public string to { get; set; }
            public string textMessage { get; set; }
            public string instancia { get; set; }
        }
    }
}
