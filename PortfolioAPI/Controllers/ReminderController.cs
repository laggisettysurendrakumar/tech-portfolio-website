using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Data;
using PortfolioAPI.Models;
using PortfolioAPI.Models.DTO;

namespace PortfolioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReminderController : ControllerBase
    {
        private readonly ILogger<ReminderController> _logger;
        private readonly PortfolioDbContext _context;

        public ReminderController(PortfolioDbContext context, ILogger<ReminderController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("GetAllReminderList")]
        public IActionResult GetAllReminderList()
        {
            try
            {
                _logger.LogInformation("GetAllReminderList method invoked at {Timestamp}", DateTime.UtcNow);

                var reminders = _context.Reminders.ToList();

                if (reminders == null || !reminders.Any())
                {
                    _logger.LogWarning("No reminders found at {Timestamp}", DateTime.UtcNow);
                    return NotFound("No reminders found.");
                }

                _logger.LogInformation("Successfully retrieved {ReminderCount} reminders at {Timestamp}", reminders.Count, DateTime.UtcNow);

                return Ok(reminders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the reminder list at {Timestamp}", DateTime.UtcNow);
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize]
        [HttpPost("AddReminder")]
        public IActionResult AddReminder(Reminder reminder)
        {
            if (reminder == null)
            {
                _logger.LogError("AddReminder failed: Reminder form data was null.");
                return BadRequest("Reminder form data is required.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("AddReminder failed: ModelState is invalid.");
                return BadRequest(ModelState);
            }

            try
            {
                _context.Reminders.Add(reminder);
                _context.SaveChanges();

                _logger.LogInformation("Reminder successfully added for user: {UserId} at {Timestamp}", User.Identity.Name, DateTime.UtcNow);

                return Ok(reminder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding reminder for user: {UserId}", User.Identity.Name);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
