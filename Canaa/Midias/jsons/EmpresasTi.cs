using Canaa.AppHost.utils;
using Canaa.FN.BusinessComponents.Usuarios;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Context; // Certifique-se de usar o namespace do seu DbContext
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Canaa.Infra.ExternalServices.Utils;

namespace Canaa.Midias.jsons
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EmpresasTi : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;

        public EmpresasTi(IWebHostEnvironment env, ApplicationDbContext context)
        {
            _env = env;
            _context = context;
        }

        [HttpGet]
        [Route("todas")]
        public async Task<IActionResult> GetTodas()
        {
            var filePath = Path.Combine(_env.ContentRootPath, "Midias", "jsons", "EmpresasVagas.json");

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { error = "Arquivo EmpresasVagas.json não encontrado.", path = filePath });

            var json = await System.IO.File.ReadAllTextAsync(filePath);

            try
            {
                // Valida JSON sem alterar o conteúdo original
                JsonSerializer.Deserialize<object>(json);
                return new ContentResult
                {
                    Content = json,
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 200
                };
            }
            catch (JsonException)
            {
                return StatusCode(500, new { error = "Conteúdo JSON inválido em EmpresasVagas.json." });
            }
        }

        [HttpGet]
        [Route("contatos")]
        public async Task<IActionResult> GetContatos()
        {
            var component = BusinessComponent.CreateInstance<IUsuariosBusiness>();
            var email = await component.BuscarTodosContatosEmail();
            return new ContentResult
            {
                Content = email.ToString(),
                ContentType = "application/json; charset=utf-8",
                StatusCode = 200
            };
        }

        [HttpPost]
        [Route("inserir-contatos")]
        public async Task<IActionResult> InserirContatos()
        {
            try
            {
                var listaParaInserir = await PrepararContatosEmailParaInserirAsync();

                if (string.IsNullOrEmpty(listaParaInserir))
                    return BadRequest(new { message = "Nenhum registro válido para inserir." });
                 
              // var component = BusinessComponent.CreateInstance<IUsuariosBusiness>();
                // var resultado = await component.InserirContatosEmailDoJson(listaParaInserir);

                return Ok(new { message = "nao esta funcionando " });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Erro ao inserir registros no banco.", details = ex.Message });
            }
        }

        private async Task<string> PrepararContatosEmailParaInserirAsync()
        {

            try
            {
                var filePath = Path.Combine(_env.ContentRootPath, "Midias", "jsons", "EmpresasVagas.json");

                if (!System.IO.File.Exists(filePath))
                {
                    Console.WriteLine($"Arquivo não encontrado: {filePath}");
                    return string.Empty;
                }

                var json = await System.IO.File.ReadAllTextAsync(filePath);
                return json;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar JSON: {ex.Message}");
                return string.Empty;
            }
        }
    }

 
}
