using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string CompanyName { get; set; }
        
        public string? Amount { get; set; }
        public string? Description { get; set; }
        public bool? Done { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
