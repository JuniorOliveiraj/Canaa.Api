using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Midias.Tarefas
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TarefasController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        [Route("Todas")]
        public IActionResult GetStatus()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<IControleTarefas>();
            var response = tarefasComponent.GetTarefasPendentes();
            return Ok(response);
        }

        [HttpGet]
        [Route("Tabela")]
        public IActionResult TabelasResult()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<IControleTarefas>();
            var response = tarefasComponent.tabela();

            return Ok(response);

        }
        [HttpGet]
        [Route("Usuarios2")]
        public async Task<IActionResult> Usuarios()
        {
            

            var tarefasComponent = BusinessComponent.CreateInstance<IControleTarefas>();
            var response = await tarefasComponent.BuscarPorIdAsync(9);

            return Ok(response); 
        }


    }
}
