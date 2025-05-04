using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models
{
    public class ContactFormModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }    // Required  
        public string Email { get; set; }   // Required, valid email  
        public string Message { get; set; } // Required  
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
