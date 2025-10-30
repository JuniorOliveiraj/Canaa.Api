using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Service
{
    public class EmailRequest
    {
    }
    /// <summary>
    /// DTO para requisição de envio de email
    /// </summary>
    public class EmailRequestDto
    {
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public List<string> Bcc { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
        public string TextBody { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public List<EmailAttachmentDto> Attachments { get; set; }
    }

    /// <summary>
    /// DTO para anexos de email
    /// </summary>
    public class EmailAttachmentDto
    {
        public string FileName { get; set; }
        public string ContentBase64 { get; set; }
    }

    /// <summary>
    /// DTO de resposta para envio de email
    /// </summary>
    public class EmailResponseDto
    {
        public bool Success { get; set; }
        public string EmailId { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public int? StatusCode { get; set; }
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime CompletedAt { get; set; }
        public double DurationMs { get; set; }
        public string RawResponse { get; set; }
        public string Exception { get; set; }
    }

    /// <summary>
    /// DTO de resposta para envio em lote
    /// </summary>
    public class BatchEmailResponseDto
    {
        public bool Success { get; set; }
        public int TotalEmails { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public string Message { get; set; }
        public List<EmailResponseDto> Results { get; set; }
    }



    public class ResendEmailDataDto
    {
        [JsonPropertyName("object")]
        public string ObjectType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("to")]
        public List<string> To { get; set; }

        [JsonPropertyName("from")]
        public string From { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }


        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("bcc")]
        public List<string> Bcc { get; set; }

        [JsonPropertyName("cc")]
        public List<string> Cc { get; set; }

        [JsonPropertyName("reply_to")]
        public List<string> ReplyTo { get; set; }

        [JsonPropertyName("last_event")]
        public string LastEvent { get; set; }

        [JsonPropertyName("scheduled_at")]
        public DateTime? ScheduledAt { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

}
