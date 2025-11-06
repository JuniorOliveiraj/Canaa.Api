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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeExplode.Channels;
using static System.Net.Mime.MediaTypeNames;

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

            for (int i = 0; i < listaDeEmails.Count; i++)
            {
                var email = CriarEmailRequest(listaDeEmails[i]);
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
                contato.StatusEmailPrincipal = true;
                ZContatosEmail.Save(contato);

                Console.WriteLine($"Email para {contato.EmailPrincipal} enviado com sucesso.");
                Console.WriteLine("Email enviado com sucesso.");
            }
            else
            {
                contato.StatusEmailPrincipal = false;
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
 
        private EmailRequestDto CriarEmailRequest(ContatosEmail contato)
        {
            var email = contato.EmailPrincipal;
            var request = new EmailRequestDto
            {
                To = new List<string> { email },
                Subject = "Candidatura para vagas em aberto",
                HtmlBody = EmailPadrao(contato.NomeEmpresa),
                Tags = new Dictionary<string, string> { { "type", "corporate_mass_email" } }
            };
            return request;
        }




        private List<ContatosEmail> BuscarListaDeEmailsAsync()
        {
            try
            {
                Criteria criteria = new Criteria("StatusEmailComercial", false);
                var emails = ZContatosEmail.GetMany(criteria);

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
            var stryle = @"    <style>
        /* Estilos gerais */
        body {
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }
        table {
            border-collapse: collapse;
        }
        td {
            font-family: Arial, sans-serif;
            color: #333333;
        }
        p {
            font-size: 16px;
            line-height: 1.6;
            margin: 0 0 20px 0;
        }
        a {
            /* Cor azul padrão para links, mas garantindo aqui */
            color: #0056b3; 
        }
        /* Estilo para tornar o Preheader invisível no corpo do e-mail */
        .preheader {
            display: none !important;
            visibility: hidden;
            opacity: 0;
            color: transparent;
            height: 0;
            width: 0;
            mso-hide: all; /* Para Outlook */
            max-height: 0px;
            overflow: hidden;
            font-size: 0px;
            line-height: 0px;
        }
    </style>";


            var EmailHtmlString = $@"
<!DOCTYPE html>
<html lang=""pt-br"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Candidatura Desenvolvedor Full-Stack - Júnior Oliveira</title>
    {stryle}
</head>
<body style=""margin: 0; padding: 0; background-color: #f4f4f4;"">

    <!-- INÍCIO: PREHEADER (Pré-visualização do e-mail na caixa de entrada) -->
    <div class=""preheader"">
        Candidatura para vaga de Desenvolvedor Full-Stack | .NET e React - Júnior Oliveira.
    </div>
    <!-- FIM: PREHEADER -->

    <table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f4f4f4;"">
        <tr>
            <td align=""center"">
                
                <table width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" style=""width: 100%; max-width: 600px; margin: 20px auto; background-color: #ffffff; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);"">

                    <tr>
                        <td>
                            <img src=""https://app.juniorbelem.com/static/mock-images/covers/CapaJuniorBelemL.jpeg"" alt=""Imagem de Capa"" width=""600"" style=""display: block; width: 100%; max-width: 600px; height: auto;"">
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding: 40px;"">
                            
                            <h1 style=""font-size: 22px; font-weight: bold; color: #222222; margin: 0 0 25px 0;"">
                                Olá, {nomeEmpresa} / Equipe de Recrutamento,
                            </h1>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Meu nome é <strong>Júnior Oliveira</strong> e sou desenvolvedor web full-stack com experiência em <strong>C#, .NET e React</strong>, atuando em projetos que vão desde o levantamento de requisitos até a entrega de soluções escaláveis e com excelente experiência para o usuário. Tenho também uma sólida base em design, permitindo criar interfaces intuitivas e eficazes.
                            </p>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 30px 0;"">
                                Estou interessado em contribuir com minha experiência e habilidades para o crescimento da sua empresa, desenvolvendo soluções inovadoras e de alto impacto.
                            </p>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 10px 0;"">
                                Meus links profissionais:
                            </p>
                            
                            <ul style=""font-size: 16px; line-height: 1.8; margin: 0 0 25px 20px; padding-left: 20px;"">
                                <li style=""margin-bottom: 10px;"">
                                    <a href=""http://juniorbelem.com"" target=""_blank"" style=""color: #0056b3; text-decoration: underline; font-weight: bold;"">
                                        Site pessoal: juniorbelem.com
                                    </a>
                                </li>
                                <li style=""margin-bottom: 10px;"">
                                    <a href=""https://www.linkedin.com/in/junior-oliveira-ba22381a3/"" target=""_blank"" style=""color: #0056b3; text-decoration: underline; font-weight: bold;"">
                                        LinkedIn: linkedin.com/in/junioroliveiraj
                                    </a>
                                </li>
                                <li style=""margin-bottom: 10px;"">
                                    <a href=""http://github.com/JuniorOliveiraj"" target=""_blank"" style=""color: #0056b3; text-decoration: underline; font-weight: bold;"">
                                        GitHub: github.com/JuniorOliveiraj
                                    </a>
                                </li>
                                <li style=""margin-bottom: 10px;"">
                                    <a href=""https://drive.google.com/file/d/1D6_LTKU4LImXm_H4ER6jlv2FrFTMY9Om/view"" target=""_blank"" style=""color: #0056b3; text-decoration: underline; font-weight: bold;"">
                                        Currículo: drive.google.com
                                    </a>
                                </li>
                            </ul>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Anexo, envio meu currículo completo para sua análise.
                            </p>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Fico à disposição para uma conversa, caso queiram conhecer melhor meu trabalho e como posso contribuir para a equipe.
                            </p>
                            
                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 20px 0;"">
                                Agradeço desde já pelo tempo e atenção.
                            </p>

                            <p style=""font-size: 16px; line-height: 1.6; margin: 0 0 0 0;"">
                                Atenciosamente,
                            </p>
                            <p style=""font-size: 18px; line-height: 1.6; font-weight: bold; color: #222222; margin: 0 0 20px 0;"">
                                Júnior Oliveira
                            </p>
                        </td>
                    </tr>

                    <tr>
                        <td style=""padding: 25px 40px; background-color: #f9f9f9; border-top: 1px solid #eeeeee;"">
                            
                            <p style=""font-size: 14px; color: #555555; text-align: center; margin: 0 0 15px 0;"">
                                (49) 99813-9167 | <a href=""mailto:junioroliveira.belem@gmail.com"" style=""color: #0056b3;"">junioroliveira.belem@gmail.com</a>
                            </p>

                            <p style=""font-size: 12px; color: #888888; text-align: center; margin: 0;"">
                                Este é um e-mail automático enviado por Júnior Oliveira. Todos os links são confiáveis e direcionam para meus perfis profissionais.
                            </p>
                        </td>
                    </tr>

                </table></td>
        </tr>
    </table></body>
</html>
";

            return EmailHtmlString;

        }
    }
    internal class StatusEnvioEmail
    {
        public int TotalSucesso { get; set; }
        public int TotalErros { get; set; }
    }
}
