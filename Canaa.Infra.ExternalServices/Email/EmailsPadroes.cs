using Canaa.DataContracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Email
{
    public static class EmailsPadroes
    {
        /// <summary>
        /// Envia um email de boas-vindas
        /// </summary>
        public static async Task<EmailResponseDto> SendWelcomeEmailAsync(string toEmail, string userName)
        {
            var request = new EmailRequestDto
            {
                To = new List<string> { toEmail },
                Subject = $"Bem-vindo à TEC, {userName}!",
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2563eb;'>Olá {userName}!</h1>
                        <p>Seja muito bem-vindo(a) à TEC! Estamos felizes em tê-lo(a) conosco.</p>
                        <p>Agora você tem acesso a todas as funcionalidades da nossa plataforma.</p>
                        <p style='margin-top: 30px;'>Atenciosamente,<br><strong>Equipe TEC</strong></p>
                    </div>",
                Tags = new Dictionary<string, string> { { "type", "welcome" } }
            };

            return await EmailService.SendEmailInternalAsync(request);
        }

        /// <summary>
        /// Envia um email de recuperação de senha
        /// </summary>
        public static async Task<EmailResponseDto> SendPasswordResetEmailAsync(string toEmail, string resetToken, string resetUrl)
        {
            var fullResetUrl = $"{resetUrl}?token={resetToken}";

            var request = new EmailRequestDto
            {
                To = new List<string> { toEmail },
                Subject = "Recuperação de Senha - TEC",
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2563eb;'>Recuperação de Senha</h1>
                        <p>Você solicitou a recuperação de senha da sua conta TEC.</p>
                        <p>Clique no botão abaixo para redefinir sua senha:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{fullResetUrl}' 
                               style='background-color: #2563eb; color: white; padding: 12px 30px; 
                                      text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Redefinir Senha
                            </a>
                        </div>
                        <p style='color: #666; font-size: 14px;'>Este link expira em 1 hora.</p>
                    </div>",
                Tags = new Dictionary<string, string> { { "type", "password_reset" } }
            };

            return await EmailService.SendEmailInternalAsync(request);
        }

        /// <summary>
        /// Envia um email de verificação de conta
        /// </summary>
        public static async Task<EmailResponseDto> SendVerificationEmailAsync(string toEmail, string verificationToken, string verificationUrl)
        {
            var fullVerificationUrl = $"{verificationUrl}?token={verificationToken}";

            var request = new EmailRequestDto
            {
                To = new List<string> { toEmail },
                Subject = "Verifique sua conta - TEC",
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <h1 style='color: #2563eb;'>Verificação de Conta</h1>
                        <p>Obrigado por se cadastrar na TEC!</p>
                        <p>Para completar seu cadastro, clique no botão abaixo:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{fullVerificationUrl}' 
                               style='background-color: #16a34a; color: white; padding: 12px 30px; 
                                      text-decoration: none; border-radius: 5px; display: inline-block;'>
                                Verificar Email
                            </a>
                        </div>
                        <p style='color: #666; font-size: 14px;'>Este link expira em 24 horas.</p>
                    </div>",
                Tags = new Dictionary<string, string> { { "type", "verification" } }
            };

            return await EmailService.SendEmailInternalAsync(request);
        }

    }
}
