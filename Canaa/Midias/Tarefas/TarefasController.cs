using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Midias.Tarefas
{
    [ApiController]
    [Route("v1/[controller]")]
    public class TarefasController: ControllerBase
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

    }
}
