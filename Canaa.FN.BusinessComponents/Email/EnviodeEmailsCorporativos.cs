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
using MySqlX.XDevAPI.Common;
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

        public async Task<ResponseDataContrac> EnvioDeEmailEmMassaCorporativos(string guid, long idUsuario)
        {
            _guid = guid;

            try
            {
                ProgressService.InsertNewTask(_guid, "Email_Corporativo");
                ProgressService.UpdateTaskStatus(_guid, "Iniciando envio de emails...");

                var listaDeEmails = BuscarListaDeEmailsAsync();
                if (listaDeEmails == null || listaDeEmails.Count <= 0)
                {

                    ProgressService.UpdateTaskError(string.Empty, "Nenhum email encontrado para envio.");

                    return MontarResponse(false, "Nenhum email encontrado para envio.", "");
                }

                if (!VerificarRolesDoUsuario(idUsuario))
                {
                    ProgressService.UpdateTaskError(_guid, "Usuário sem permissão para envio.");
                    return MontarResponse(false, "Usuário não possui permissão para executar esta ação.");
                }

                var resultado = await EnviarEmailParaListaDeEmails(listaDeEmails);

                var mensagemFinal = @$"Envio de emails concluído. Total de enviados: {resultado.TotalSucesso}, falhas: {resultado.TotalErros}";
                ProgressService.UpdateTaskStatus(_guid, mensagemFinal);
                return MontarResponse(true, mensagemFinal);
            }
            catch (Exception ex)
            {
                return MontarResponse(false, "Erro ao iniciar o processo de envio de emails.", ex.Message);
            }

        }

        private ResponseDataContrac MontarResponse(bool sucesso, string mensagem, string erros = "")
        {
            if (!sucesso)
                ProgressService.UpdateTaskError(_guid, "Usuário sem permissão para envio.");


            ProgressService.SetConcluido(_guid);

            var response = new ResponseDataContrac
            {
                success = sucesso,
                message = mensagem,
                data = _guid,
                error = erros,
                status = sucesso ? "Concluído" : "Erros"
            };
            ProgressService.SetConcluido(_guid);
            return response;
        }

        private bool VerificarRolesDoUsuario(long idUsuario)
        {
             var usuario = ZUsuarios.GetFirstOrDefault(new Criteria("ID", idUsuario));
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
            const int tamanhoLote = 10;
            int atrasoEntreLotesMs = 10000;
            int atrasoMinimoMs = 500;
            int atrasoMaximoMs = 1500;


            var random = new Random();

            for (int i = 0; i < 50; i++)
            {
                var email = CriarEmailRequest(listaDeEmails[i], i + 1);
                await Task.Delay(3000);// var resultadoDoEnvio = await EmailService.SendEmailAsync(email);

                //results.Add(resultadoDoEnvio);
                //await VerificarStatusDoEnvio(resultadoDoEnvio)
                if (true)
                {
                    successCount++;
                    atualizarSituacaoEmail(true, listaDeEmails[i].Id);
                }
                else
                {
                    failedCount++;
                    atualizarSituacaoEmail(false, listaDeEmails[i].Id);
                }

                 double progresso = ((double)(i + 1) / listaDeEmails.Count) * 100;
                ProgressService.UpdateTaskProgress(_guid, progresso);

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
                contato.StatusEmailComercial = true;
                ZContatosEmail.Save(contato);

                Console.WriteLine($"Email para {contato.EmailPrincipal} enviado com sucesso.");
                Console.WriteLine("Email enviado com sucesso.");
            }
            else
            {
                contato.StatusEmailComercial = false;
                ZContatosEmail.Save(contato);
            }

        }

        private async Task<bool> VerificarStatusDoEnvio(EmailResponseDto resultadoDoEnvio)
        {
            if (resultadoDoEnvio == null || string.IsNullOrEmpty(resultadoDoEnvio.EmailId))
                return false;

            // Aguarda 3 segundos antes de consultar o status
            await Task.Delay(3000);

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
                HtmlBody = EmailPadrao(contato.NomeEmpresa),
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


        private string EmailPadrao(string nomeEmpresa)
        {
            return $@"<!DOCTYPE html>
                            <html lang=""pt-BR"">
                            <head>
                            <meta charset=""UTF-8"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Email Candidatura</title>
                            <style>
                              body {{
                                font-family: Arial, sans-serif;
                                color: #333333;
                                line-height: 1.5;
                                background-color: #f9f9f9;
                                margin: 0;
                                padding: 20px;
                              }}
                              .container {{
                                max-width: 600px;
                                margin: auto;
                                background-color: #ffffff;
                                padding: 30px;
                                border-radius: 8px;
                                box-shadow: 0 0 10px rgba(0,0,0,0.1);
                              }}
                              h2 {{
                                color: #0073e6;
                              }}
                              a {{
                                color: #0073e6;
                                text-decoration: none;
                              }}
                              a:hover {{
                                text-decoration: underline;
                              }}
                              .footer {{
                                margin-top: 20px;
                                font-size: 0.9em;
                                color: #777777;
                              }}
                            </style>
                            </head>
                            <body>
                              <div class=""container"">
                                <h2>Olá {nomeEmpresa},</h2>
                                <p>Meu nome é <strong>Júnior Oliveira</strong> e sou desenvolvedor web full-stack com experiência em <strong>C#, .NET e React</strong>, atuando em projetos que vão desde o levantamento de requisitos até a entrega de soluções escaláveis e com excelente experiência para o usuário. Tenho também uma sólida base em design, permitindo criar interfaces intuitivas e eficazes.</p>
    
                                <p>Estou interessado em contribuir com minha experiência e habilidades para o crescimento da sua empresa, desenvolvendo soluções inovadoras e de alto impacto.</p>
    
                                <p><strong>Meus links profissionais:</strong><br>
                                  - <strong>Site pessoal:</strong> <a href=""https://www.juniorbelem.com"" target=""_blank"">juniorbelem.com</a><br>
                                  - <strong>LinkedIn:</strong> <a href=""https://www.linkedin.com/in/junior-oliveira-ba22381a3/"" target=""_blank"">linkedin.com/in/junioroliveiraj</a><br>
                                  - <strong>GitHub:</strong> <a href=""https://github.com/JuniorOliveiraj"" target=""_blank"">github.com/JuniorOliveiraj</a><br>
      
                                  - <strong>Curriculo:</strong> <a href=""https://drive.google.com/file/d/1D6_LTKU4LImXm_H4ER6jlv2FrFTMY9Om/view?usp=sharing"" target=""_blank"">drive.google.com</a><br>
                                </p>
    
                                <p>Anexo, envio meu currículo completo para sua análise.</p>
    
                                <p>Fico à disposição para uma conversa, caso queiram conhecer melhor meu trabalho e como posso contribuir para a equipe.</p>
    
                                <p>Agradeço desde já pelo tempo e atenção.</p>
    
                                <p>Atenciosamente,<br>
                                <strong>Júnior Oliveira</strong><br>
                                (49) 99813-9167 | junioroliveira.belem@gmail.com</p>
    
                                <div class=""footer"">
                                  Este é um e-mail automático enviado por Júnior Oliveira. Todos os links são confiáveis e direcionam para meus perfis profissionais.
                                </div>
                              </div>
                            </body>
                            </html>
";
        }
    }
    internal class StatusEnvioEmail
    {
        public int TotalSucesso { get; set; }
        public int TotalErros { get; set; }
    }
}
