using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models
{
    public class AdminLoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
