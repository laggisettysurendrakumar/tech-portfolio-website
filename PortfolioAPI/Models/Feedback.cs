using System.ComponentModel.DataAnnotations;

namespace PortfolioAPI.Models
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Relationship { get; set; } = string.Empty;

        [Range(1, 5)]
        public int CommunicationRating { get; set; }

        [Range(1, 5)]
        public int CollaborationRating { get; set; }

        [Range(1, 5)]
        public int TechnicalSkillRating { get; set; }

        [Range(1, 5)]
        public int CodeQualityRating { get; set; }

        [Range(1, 5)]
        public int HelpfulnessRating { get; set; }

        public string? WhatWentWell { get; set; }
        public string? WhatCouldBeImproved { get; set; }
        public string? FarewellNote { get; set; }

        [Required]
        public string YourName { get; set; } = string.Empty;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
