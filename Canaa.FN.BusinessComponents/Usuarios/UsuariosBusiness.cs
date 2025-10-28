using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.ExternalServices.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Usuarios
{
    public class UsuariosBusiness : IUsuariosBusiness
    {
        private readonly ApplicationDbContext _context;

        public UsuariosBusiness(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Z_USUARIO> BuscarPorIdAsync(int id)
        {
            return await _context.Z_USUARIOs.FindAsync(id);
        }
        public async Task<List<ContatosEmail>> BuscarTodosContatosEmail()
        {
            try
            {
                var emails = await _context.ContatosEmail.ToListAsync();
                Console.WriteLine($"Total registros: {emails.Count}");
                return emails;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao ler ContatosEmail:");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.ToString());
                throw;

                return new List<ContatosEmail>();
            }
        }

        public async Task<string> InserirContatosJson(string json)
        {
            try
            {
                return await InserirContatosEmailDoJson(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao ler ContatosEmail:");
                Console.WriteLine(ex.ToString());


                return ex.ToString();
            }
        }


        private async Task<string> InserirContatosEmailDoJson(string json)
        {
            try
            {
                var listaEmails = MontarJson(json);

                if (listaEmails == null || !listaEmails.Any())
                    return "JSON vazio ou inválido.";

                // Inserção em lotes de 50 registros para evitar travamento do MySQL
                int batchSize = 50;
                int totalInseridos = 0;

                for (int i = 0; i < listaEmails.Count; i += batchSize)
                {
                    var batch = listaEmails.Skip(i).Take(batchSize).ToList();
                    _context.ContatosEmail.AddRange(batch);
                    totalInseridos += batch.Count;
                    await _context.SaveChangesAsync();
                }

                return $"{totalInseridos} registros inseridos com sucesso!";
            }
            catch (JsonException jex)
            {
                return $"Erro ao processar JSON: {jex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (ex.InnerException != null)
                    Console.WriteLine(ex.InnerException.ToString());
                throw;
            }
        }

        Task<string> IUsuariosBusiness.InserirContatosEmailDoJson(string json)
        {
            return InserirContatosEmailDoJson(json);
        }

        private List<ContatosEmail> MontarJson(string json)
        {
            var listaTratada = new List<ContatosEmail>();

            var jsonElement = JsonSerializer.Deserialize<List<JsonElement>>(json);

            if (jsonElement == null || !jsonElement.Any())
            {
                Console.WriteLine("JSON vazio ou inválido.");
                return listaTratada;
            }

            foreach (var item in jsonElement)
            {
                // Ler campos do JSON com segurança
                string nomeEmpresa = Truncar(item.GetPropertyOrDefault("nome") ?? "Empresa Associada", 150);
                string siteLink = Truncar(item.GetPropertyOrDefault("link") ?? "https://www.seprosc.com.br/", 255);
                string imagem = Truncar(item.GetPropertyOrDefault("imagem") ?? "", 255);
                string dominio = Truncar(item.GetPropertyOrDefault("dominio") ?? "", 150);
                string emailRh = Truncar(item.GetPropertyOrDefault("email_rh") ?? "contato@empresa.com", 150);

                var contato = new ContatosEmail
                {
                    NomeEmpresa = nomeEmpresa,
                    SiteLink = siteLink,
                    Imagem = "https://www.seprosc.com.br/"+imagem,
                    Dominio = dominio,
                    EmailPrincipal = emailRh,
                    EmailComercial = emailRh.Replace("rh@", "contato@"),
                    Observacoes = "Empresa associada de tecnologia.",
                    Categoria = Truncar(item.GetPropertyOrDefault("categoria") ?? "Tecnologia", 100),
                    Cidade = Truncar(item.GetPropertyOrDefault("cidade") ?? "Blumenal", 100),
                    TipoContato = "Empresa Associada",
                    // Id não definido, banco gera automaticamente
                };

                listaTratada.Add(contato);
            }

            return listaTratada;
        }

        // Função auxiliar para truncar strings
        private string Truncar(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }

 
}
