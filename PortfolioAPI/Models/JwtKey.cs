using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models
{
    public class JwtKey
    {
        [Key]
        public int Id { get; set; }
        public string KeyVersion { get; set; } = string.Empty; // e.g., "2025050513"
        public string EncryptedKey { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
