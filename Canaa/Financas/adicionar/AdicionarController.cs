using Canaa.AppHost.utils;
using Canaa.DataContracts.Gastos;
using Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos;
using Canaa.FN.BusinessComponents.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Financas.adicionar
{
    [Authorize]
    [Route("v1/[controller]")]
    public class AdicionarController : ControllerBase
    {
        [HttpPost]
        [HttpPost("Gastos-MercadoPago")]
        public IActionResult GastosMercadoPago([FromBody] List<GastosMercadoPagoDataContract> gastos)
        {
            var gastosComponent = BusinessComponent.CreateInstance<IAdicionarJsonGastosMercadoPago>();
             ResponseDataContrac result =  gastosComponent.AdicionarComJson(gastos);
            return Ok(result);
        }
    }
    
    
}
