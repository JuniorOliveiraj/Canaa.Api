using Canaa.DataContracts.Service;
using Canaa.DataContracts.Whatsapp;
using Canaa.FN.BusinessComponents.Response;
using Canaa.FN.BusinessComponents.Utils;
using Canaa.Infra.Entities.Context;
using Canaa.Infra.Entities.Entities;
using Canaa.Infra.Entities.Tabelasbef;
using Canaa.Infra.Entities.Utils;
using Canaa.Infra.ExternalServices.Email;
using Canaa.Infra.ExternalServices.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Channels;

namespace Canaa.FN.BusinessComponents.Email
{
    public class EnviodeEmailsCorporativos : IEnviodeEmailsCorporativos
    {
        private string _guid;


        public ResponseDataContrac EnvioDeEmailEmMassaCorporativos()
        {
            _guid = Guid.NewGuid().ToString();
            ProgressService.InsertNewTask(_guid, "Email_Corporativo");
            ProgressService.UpdateTaskStatus(_guid, "Iniciando");

            var listaDeEmails = BuscarListaDeEmailsAsync();
            if (listaDeEmails.Count <= 0)
                return MontarResponse(false, "Iniciando envio de emails corporativos em massa.");

            if (!VerificarRolesDoUsuario())
                return MontarResponse(false, "Usuário não possui permissão para executar esta ação.");

            var envioTask = EnviarEmailParaListaDeEmails(listaDeEmails);
            var mensagemFinal = @$"Envio de email Concluido Total de Emails enviados: ${envioTask.Result.TotalSucesso} total com falhas $${envioTask.Result.TotalErros}";
            return MontarResponse(true, mensagemFinal);
        }

        private ResponseDataContrac MontarResponse(bool sucesso, string mensagem)
        {
            ResponseDataContrac response = new ResponseDataContrac();
            response.status = sucesso ? "Concluído" : "Erros";
            response.success = sucesso;
            response.message = mensagem;
            return response;
        }

        private bool VerificarRolesDoUsuario()
        {
            int usuarioId = CanaaContext.GetUserId();
            var usuario = ZUsuarios.GetFirstOrDefault(new Criteria("ID", usuarioId));
            if (usuario != null)
            {
                var roles = usuario.PAPEL;
                return roles.Contains("ADM");
            }
            return false;
        }
        private async Task<StatusEnvioEmail> EnviarEmailParaListaDeEmails(List<ContatosEmail> listaDeEmails)
        {
            var results = new List<EmailResponseDto>();
            var failedCount = 0;
            var successCount = 0;

            // Parâmetros de controle
            const int tamanhoLote = 2;
            int atrasoEntreLotesMs = 10000;
            int atrasoMinimoMs = 500;
            int atrasoMaximoMs = 1500;


            var random = new Random();

            for (int i = 0; i < 2; i++)
            {
                int emailsProcessados = i + 1;
                var email = CriarEmailRequest(listaDeEmails[i], emailsProcessados);
                // Por esta linha:
                var resultadoDoEnvio = await EmailService.SendEmailAsync(email);


                results.Add(resultadoDoEnvio);

                if (await VerificarStatusDoEnvio(resultadoDoEnvio))
                {
                    successCount++;
                    atualizarSituacaoEmail(true, listaDeEmails[i].Id);
                }
                else
                {
                    failedCount++;
                    atualizarSituacaoEmail(false, listaDeEmails[i].Id);
                }
                await Task.Delay(random.Next(atrasoMinimoMs, atrasoMaximoMs));

                if ((i + 1) % tamanhoLote == 0 && i + 1 < listaDeEmails.Count)
                {
                    Console.WriteLine($"Aguardando {atrasoEntreLotesMs / 1000}s antes do próximo lote...");
                    await Task.Delay(atrasoEntreLotesMs);
                }
            }
            ProgressService.UpdateTaskStatus(_guid, "Concluído");

            return new StatusEnvioEmail
            {
                TotalSucesso = successCount,
                TotalErros = failedCount
            };

        }


        private void atualizarSituacaoEmail(bool statusEnvio, long id)
        {
            var criteria = new Criteria("ID", id);

            var contato = ZContatosEmail.GetForEdit(criteria);
            if (statusEnvio)
            {
                contato
                Console.WriteLine($"Email para {contato.EmailPrincipal} enviado com sucesso.");
                Console.WriteLine("Email enviado com sucesso.");
            }
            else
            {

            }

        }

        private async Task<bool> VerificarStatusDoEnvio(EmailResponseDto resultadoDoEnvio)
        {
            if (resultadoDoEnvio == null || string.IsNullOrEmpty(resultadoDoEnvio.EmailId))
                return false;

            // Aguarda 2 segundos antes de consultar o status
            await Task.Delay(2000);

            var dadosEnvio = await EmailService.GetEmailStatusAsync(resultadoDoEnvio.EmailId);

            if (dadosEnvio.LastEvent != null && dadosEnvio.LastEvent.Contains("sent", StringComparison.OrdinalIgnoreCase))
                return false;

            return resultadoDoEnvio.Success;
        }



        private bool VerificaSeEimpar(int numero)
        {
            return numero % 2 != 0;
        }

        private EmailRequestDto CriarEmailRequest(ContatosEmail contato, int contagemEnvio)
        {
            var email = "emailFalso@t.com";
            if (VerificaSeEimpar(contagemEnvio))
            {
                email = "juniorbelemj@gmail.com";
            }
            var request = new EmailRequestDto
            {
                To = new List<string> { email },
                Subject = "Assunto do Email Corporativo",
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2563eb;'>Olá {contato.NomeEmpresa}!</h1>
                        <p>Este é um email corporativo enviado em massa.</p>
                        <p>Atenciosamente,<br><strong>Equipe Corporativa</strong></p>
                    </div>",
                Tags = new Dictionary<string, string> { { "type", "corporate_mass_email" } }
            };
            return request;
        }




        private List<ContatosEmail> BuscarListaDeEmailsAsync()
        {
            try
            {
                var emails = ZContatosEmail.GetAll();

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

                return new List<ContatosEmail>();

            }
        }
    }
    internal class StatusEnvioEmail
    {
        public int TotalSucesso { get; set; }
        public int TotalErros { get; set; }
    }
}
