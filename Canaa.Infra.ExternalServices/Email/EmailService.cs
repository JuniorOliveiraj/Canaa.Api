using Canaa.DataContracts.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Email
{
    /// <summary>
    /// Serviço estático para envio de emails usando Resend API (funções básicas)
    /// </summary>
    public static class EmailService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.resend.com/"),
            Timeout = TimeSpan.FromSeconds(30)
        };

        private static string _apiKey;
        private static string _defaultFromEmail;
        private static string _defaultFromName;

        /// <summary>
        /// Configura o serviço de email com as credenciais necessárias
        /// </summary>
        public static void Configure(string apiKey, string defaultFromEmail, string defaultFromName = "TEC")
        {
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("API Key é obrigatória", nameof(apiKey));

            if (string.IsNullOrWhiteSpace(defaultFromEmail))
                throw new ArgumentException("Email padrão é obrigatório", nameof(defaultFromEmail));

            _apiKey = apiKey;
            _defaultFromEmail = defaultFromEmail;
            _defaultFromName = defaultFromName;

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Envia um email simples
        /// </summary>
        public static async Task<EmailResponseDto> SendEmailAsync(EmailRequestDto request)
        {
            ValidateConfiguration();
            return await SendEmailInternalAsync(request);
        }

        /// <summary>
        /// Envia emails em lote
        /// </summary>
        public static async Task<BatchEmailResponseDto> SendBatchEmailsAsync(List<EmailRequestDto> requests)
        {
            ValidateConfiguration();

            var results = new List<EmailResponseDto>();
            var failedCount = 0;
            var successCount = 0;

            // Parâmetros de controle
            const int batchSize = 50;
            const int minDelayMs = 2000;
            const int maxDelayMs = 6000;
            const int batchDelayMs = 30000; // 30 segundos entre lotes

            var random = new Random();

            for (int i = 0; i < requests.Count; i++)
            {
                var request = requests[i];
                var result = await SendEmailInternalAsync(request);
                results.Add(result);

                if (result.Success)
                    successCount++;
                else
                    failedCount++;

                await Task.Delay(random.Next(minDelayMs, maxDelayMs));

                // Delay maior entre lotes
                if ((i + 1) % batchSize == 0 && i + 1 < requests.Count)
                {
                    Console.WriteLine($"Aguardando {batchDelayMs / 1000}s antes do próximo lote...");
                    await Task.Delay(batchDelayMs);
                }
            }

            return new BatchEmailResponseDto
            {
                Success = failedCount == 0,
                TotalEmails = requests.Count,
                SuccessCount = successCount,
                FailedCount = failedCount,
                Results = results,
                Message = failedCount == 0
                    ? "Todos os emails foram enviados com sucesso."
                    : $"{successCount} emails enviados, {failedCount} falharam."
            };
        }


        /// <summary>
        /// Valida um endereço de email
        /// </summary>
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #region Métodos Internos

        private static void ValidateConfiguration()
        {
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("EmailService não foi configurado. Chame Configure() primeiro.");
        }

        internal static async Task<EmailResponseDto> SendEmailInternalAsync(EmailRequestDto request)
        {
            var response = new EmailResponseDto
            {
                RequestedAt = DateTime.UtcNow
            };

            try
            {
                if (request.To == null || !request.To.Any())
                {
                    response.Success = false;
                    response.ErrorMessage = "Lista de destinatários está vazia";
                    response.ErrorCode = "INVALID_RECIPIENTS";
                    return response;
                }

                if (!request.To.All(ValidateEmail))
                {
                    response.Success = false;
                    response.ErrorMessage = "Um ou mais emails de destinatários são inválidos";
                    response.ErrorCode = "INVALID_EMAIL_FORMAT";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.Subject))
                {
                    response.Success = false;
                    response.ErrorMessage = "Assunto é obrigatório";
                    response.ErrorCode = "MISSING_SUBJECT";
                    return response;
                }

                if (string.IsNullOrWhiteSpace(request.HtmlBody) && string.IsNullOrWhiteSpace(request.TextBody))
                {
                    response.Success = false;
                    response.ErrorMessage = "Corpo do email é obrigatório";
                    response.ErrorCode = "MISSING_BODY";
                    return response;
                }

                var payload = new
                {
                    from = !string.IsNullOrWhiteSpace(request.FromEmail)
                        ? $"{request.FromName ?? _defaultFromName} <{request.FromEmail}>"
                        : $"{_defaultFromName} <{_defaultFromEmail}>",
                    to = request.To.ToArray(),
                    cc = request.Cc?.ToArray(),
                    bcc = request.Bcc?.ToArray(),
                    subject = request.Subject,
                    html = request.HtmlBody,
                    text = request.TextBody,
                    reply_to = request.ReplyTo,
                    tags = request.Tags,
                    attachments = request.Attachments?.Select(a => new
                    {
                        filename = a.FileName,
                        content = a.ContentBase64
                    }).ToArray()
                };

                var jsonPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync("emails", content);
                var responseContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var resendResponse = JsonSerializer.Deserialize<ResendApiResponse>(responseContent);

                    response.Success = true;
                    response.EmailId = resendResponse?.Id;
                    response.Message = "Email enviado com sucesso";
                    response.Recipients = request.To;
                    response.Subject = request.Subject;
                }
                else
                {
                    var errorResponse = JsonSerializer.Deserialize<ResendErrorResponse>(responseContent);

                    response.Success = false;
                    response.ErrorMessage = errorResponse?.Message ?? "Erro desconhecido ao enviar email";
                    response.ErrorCode = $"HTTP_{(int)httpResponse.StatusCode}";
                    response.StatusCode = (int)httpResponse.StatusCode;
                    response.RawResponse = responseContent;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessage = $"Erro inesperado: {ex.Message}";
                response.ErrorCode = "UNEXPECTED_ERROR";
                response.Exception = ex.ToString();
            }
            finally
            {
                response.CompletedAt = DateTime.UtcNow;
                response.DurationMs = (response.CompletedAt - response.RequestedAt).TotalMilliseconds;
            }

            return response;
        }

        /// <summary>
        /// Obtém os detalhes completos de um email enviado (status, destinatários, etc)
        /// </summary>
        public static async Task<ResendEmailDataDto> GetEmailStatusAsync(string emailId)
        {
            ValidateConfiguration();

            if (string.IsNullOrWhiteSpace(emailId))
                throw new ArgumentException("O ID do email é obrigatório.", nameof(emailId));

            try
            {
                var response = await _httpClient.GetAsync($"emails/{emailId}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro ao consultar email {emailId}: {response.StatusCode} - {content}");
                }

                var emailData = JsonSerializer.Deserialize<ResendEmailDataDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return emailData;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao obter status do email {emailId}: {ex.Message}", ex);
            }
        }




        #endregion
    }

    #region Modelos da API Resend

    internal class ResendApiResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    internal class ResendErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    #endregion
}
