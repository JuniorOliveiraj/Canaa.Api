using Canaa.AppHost.utils;
using Canaa.DataContracts.Gastos;
using Canaa.FN.BusinessComponents.Adicionar.AdicionarGastos;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Canaa.FN.BusinessComponents.Response;

namespace Canaa.Financas.adicionar
{
    [Authorize]
    [Route("v1/[controller]")]
    public class AdicionarController : ControllerBase
    {
        [HttpPost]
        [HttpPost("Gastos-MercadoPago")]
        public async Task<IActionResult> GastosMercadoPago([FromBody] List<GastosMercadoPagoDataContract> gastos)
        {
            var gastosComponent = BusinessComponent.CreateInstance<IAdicionarJsonGastosMercadoPago>();
            var result = gastosComponent.AdicionarComJson(gastos);
            return Ok(result);
        }
    }


}
