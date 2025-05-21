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

                var reminders = _context.Reminders
                    .OrderByDescending(r => r.Done)               // Done == true first
                    .ThenByDescending(r => r.CreatedDate)         // Newest first within Done status
                    .ToList();

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
                reminder.CreatedDate = DateTime.Now;
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

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateReminder(int id, [FromBody] Reminder updatedReminder)
        {
            if (updatedReminder == null || updatedReminder.Id != id)
            {
                _logger.LogWarning("UpdateReminder failed: Invalid reminder data for id {ReminderId}", id);
                return BadRequest("Invalid reminder data.");
            }

            var existingReminder = _context.Reminders.FirstOrDefault(r => r.Id == id);

            if (existingReminder == null)
            {
                _logger.LogWarning("UpdateReminder failed: Reminder with id {ReminderId} not found.", id);
                return NotFound($"Reminder with id {id} not found.");
            }

            try
            {
                // Update the properties
                existingReminder.CompanyName = updatedReminder.CompanyName;
                existingReminder.Description = updatedReminder.Description;
                existingReminder.Amount = updatedReminder.Amount;
                existingReminder.Done = updatedReminder.Done;
                existingReminder.CreatedDate = DateTime.UtcNow; 

                _context.SaveChanges();

                _logger.LogInformation("Reminder with id {ReminderId} successfully updated.", id);
                return Ok(existingReminder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the reminder with id {ReminderId}.", id);
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


    }
}
