using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Buscar.Gastos;

//using Canaa.FN.BusinessComponents.Buscar.Gastos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Financas.GetGastos
{
    [Authorize]
    [Route("v1/[controller]")]
    public class Total : ControllerBase
    {
        [HttpGet]
        [HttpGet("Gastos")]
        public IActionResult Gastos()
        {
            var gstosComponent = BusinessComponent.CreateInstance<ITotalGastos>();
            return Ok(gstosComponent.Total());
        }
    }
}
