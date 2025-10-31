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

            // Dispara a tarefa em background, sem bloquear a resposta HTTP
            _ = Task.Run(async () =>
            {
                try
                {
                    await componente.EnvioDeEmailEmMassaCorporativos(guid, usuarioId);
                }
                catch (Exception ex)
                {
                    // Registre a exceção (log, etc.), mas não quebre o retorno ao cliente
                    Console.WriteLine($"Erro no envio de emails: {ex.Message}");
                }
            });

            // Retorna imediatamente ao cliente
            var response = new
            {
                guid,
                data = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                message = "Envio de emails iniciado com sucesso."
            };

            return Ok(response);
        }
    }
}
