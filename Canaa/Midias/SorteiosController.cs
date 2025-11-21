using Canaa.AppHost.utils;
using Canaa.DataContracts.Service;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Usuarios.sorteios;
using Canaa.TempModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Canaa.Midias
{
    [ApiController]
    [Route("v1/[controller]")]
    public class SorteiosController: ControllerBase
    {
        [HttpGet]
        [Route("participantes")]
        public IActionResult GetParticipantes()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            var response = tarefasComponent.BuscarParticipantes();
            return Ok(response);
        }

        [HttpPost]
        [Route("participantes")]
        public IActionResult SetParticipantes(string nome, string number)
        {
            var part = new ParticipantesRequest
            {
                Nome = nome,
                Telefone = number
            };
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            var response = tarefasComponent.AdicionarParticipants(part);
            return Ok(response);
        }
        [HttpDelete]
        [Route("participantes")]
        public IActionResult deletParticipantes(int participants)
        {
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            var response = tarefasComponent.DeletarParticipante(participants);
            return Ok(response);
        }

        [HttpPost]
        [Route("alterar-status")]
        public IActionResult Alterarsorteios()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            tarefasComponent.AterarStatusSorteios();
            return Ok(true);
        }
        [HttpPost]
        [Route("gerar")]
        public IActionResult GerarSorteios()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            var response = tarefasComponent.GerarNovoSorteio();
            return Ok(response);
        }
        [HttpDelete]
        [Route("apagar")]
        public IActionResult ApagarSorteios()
        {
            var tarefasComponent = BusinessComponent.CreateInstance<ISorteioBusiness>();
            tarefasComponent.ApagarSorteios();
            return Ok(true);
        }



    }
}
