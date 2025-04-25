using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;
using Canaa.DataContracts.Whatsapp;
using Canaa.Infra.ExternalServices.Whatsapp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Canaa.WhatsApp.WhatsappController;

namespace Canaa.WorkFlow.Marmitas
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CancelarMarmitas : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("cancelar-marmita")]
        public async Task<IActionResult> Cancelar()
        { 
            Query query = new Query(@"UPDATE WF_PARAM_MARMITA SET ENVIAR = 0 WHERE DIADASEMANA = :DIADASEMANA");
            query.AddParameter(new Parameter("DIADASEMANA", DateTime.Now.DayOfWeek.ToString().ToUpper() ));
            var result =  query.Execute().FirstOrDefault();


            return Ok(new
            {

                Ok = true,
                message = $"Marmitas de {DateTime.Now.DayOfWeek.ToString().ToUpper()} canceladas com sucesso"
            });
        }

    }
}
