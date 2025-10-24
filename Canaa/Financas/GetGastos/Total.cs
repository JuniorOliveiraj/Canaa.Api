using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Financas.Buscar.Gastos;


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

        [AllowAnonymous]
        [HttpGet("Gastos-public")]
        public IActionResult GastosPublico()
        {
            var gstosComponent = BusinessComponent.CreateInstance<ITotalGastos>();

            var total = gstosComponent.Total();

            var response = new
            {
                total = total,
                data = DateTime.Now.ToString("dd/MM/yyyy"),
                poderaGuarda = 1700 - total ,
                message = "Total de gastos"
            };

            return Ok(response);
        }



        [AllowAnonymous]
        [HttpGet("ultimosgastos-public")]
        public IActionResult UltimosGastosPublico()
        {
            var gstosComponent = BusinessComponent.CreateInstance<ITotalGastos>();

            var total = gstosComponent.RetornarTotalComUltimosGastos();

            var response = new
            {
                dados = total,
                data = DateTime.Now.ToString("dd/MM/yyyy"),

            };

            return Ok(response);
        }



    }
}
