using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Comunicacao.Emails
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EmailsController : ControllerBase
    {
        [Authorize]
        [HttpPost("EnviarEmMassa")]
        public IActionResult GetStatus()
        {
            var componet = BusinessComponent.CreateInstance<IEnviodeEmailsCorporativos>();
            var resultado = componet.EnvioDeEmailEmMassaCorporativos();
            return Ok(resultado);
        }
    }
}
