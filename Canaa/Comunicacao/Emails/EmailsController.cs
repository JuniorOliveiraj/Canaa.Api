using Canaa.AppHost.utils;
using Canaa.Financas.GetGastos;
using Canaa.FN.BusinessComponents.Email;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.ExternalServices.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Canaa.Comunicacao.Emails
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EmailsController : ControllerBase
    {
        [Authorize]
        [HttpPost("EnviarEmMassa")]
        public IActionResult EnvioDeEmailEmMassaCorporativos()
        {
            var guid = Guid.NewGuid().ToString();
            var componente = BusinessComponent.CreateInstance<IEnviodeEmailsCorporativos>();
            int usuarioId = CanaaContext.GetUserId();
            string mensagem = " ";

            _ = Task.Run(async () =>
            {
                try
                {
                    mensagem = "Nao ta funcionando so ADM pode enviar";
                    //await componente.EnvioDeEmailEmMassaCorporativos(guid, usuarioId);
                }
                catch (Exception ex)
                {
                    mensagem = ($"Erro no envio de emails: {ex.Message}");
                }
            });

            var response = new
            {
                guid,
                data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                message = mensagem
            };

            return Ok(response);
        }
    }
}
