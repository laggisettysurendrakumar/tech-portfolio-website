using System.Text.Json.Serialization;

namespace PortfolioAPI.Models
{
    public class MailerSendRequest
    {
        [JsonPropertyName("from")]
        public EmailAddress From { get; set; }

        [JsonPropertyName("to")]
        public List<EmailAddress> To { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
