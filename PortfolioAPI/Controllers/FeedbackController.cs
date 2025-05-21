using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Data;
using PortfolioAPI.Models;
using PortfolioAPI.Models.DTO;
using System;

namespace PortfolioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly PortfolioDbContext _context;
        private readonly ILogger<FeedbackController> _logger;

        public FeedbackController(PortfolioDbContext context, ILogger<FeedbackController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("SubmitFeedback")]
        public async Task<IActionResult> SubmitFeedback([FromBody] FeedbackDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var feedback = new Feedback
                {
                    Relationship = dto.Relationship,
                    CommunicationRating = dto.CommunicationRating,
                    CollaborationRating = dto.CollaborationRating,
                    TechnicalSkillRating = dto.TechnicalSkillRating,
                    CodeQualityRating = dto.CodeQualityRating,
                    HelpfulnessRating = dto.HelpfulnessRating,
                    WhatWentWell = dto.WhatWentWell,
                    WhatCouldBeImproved = dto.WhatCouldBeImproved,
                    FarewellNote = dto.FarewellNote,
                    YourName = dto.YourName
                };

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Feedback submitted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding feedback of user name: {Username}", dto.YourName);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("GetFeedbacks")]
        public async Task<IActionResult> GetFeedbacks()
        {
            try
            {
                var feedbacks = await _context.Feedbacks.OrderByDescending(f => f.SubmittedAt).ToListAsync();

                return Ok(feedbacks);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the feedbacks list at {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
