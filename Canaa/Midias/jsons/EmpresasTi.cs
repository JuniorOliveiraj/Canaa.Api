using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canaa.Midias.jsons
{
    [ApiController]
    [Route("v1/[controller]")]
    public class EmpresasTi : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public EmpresasTi(IWebHostEnvironment env)
        {
            _env = env;
        }

        /*
         PSEUDOCÓDIGO (plano detalhado):
         - Construir caminho absoluto do arquivo EmpresasVagas.json usando _env.ContentRootPath + "Midias/jsons/EmpresasVagas.json".
         - Verificar se o arquivo existe; se não existir, retornar NotFound (404) com mensagem.
         - Ler o conteúdo do arquivo de forma assíncrona (ReadAllTextAsync).
         - Tentar desserializar para um objeto genérico (apenas para validar o JSON).
         - Retornar o JSON original como ContentResult com content-type "application/json".
         - Em caso de erro de desserialização, retornar 500 com mensagem de erro.
        */

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
    }
}
