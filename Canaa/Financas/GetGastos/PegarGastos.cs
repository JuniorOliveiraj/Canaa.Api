
//using Canaa.Bussines.Comum.Gastos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Canaa.AppHost.Financas.GetGastos
{
    //[Authorize]
    [ApiController]
    [Route("v1/[controller]")]
    public class GastosController : ControllerBase
    {
        [HttpGet("saudacao")]
        public IActionResult Saudacao()
        {
          //  var query = new Query(@"SELECT * FROM gastos WHERE status = 1");
            //var PegarGastosComponent = BusinessComponent.CreateInstance<IClass>();

            //  var nomes = PegarGastosComponent.NomeCompraTeste();
            string nomes = "Olá, Mundo!";
            if (nomes != null)
            {
               // var nomes = nomes.Select(r => r["compra_nome"].ToString()).ToList();
                return Ok(new
                {
                    mensagem = nomes,
                    nomes
                });
            }
            return Ok("DEU RUIM");
        }


    }
}
