using System.Text.Json.Serialization;

namespace PortfolioAPI.Models
{
    public class EmailAddress
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
