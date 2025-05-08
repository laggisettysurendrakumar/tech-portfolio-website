using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models.DTO
{
    public class ContactFormDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public DateTime SubmittedAt { get; set; }
    }
}
