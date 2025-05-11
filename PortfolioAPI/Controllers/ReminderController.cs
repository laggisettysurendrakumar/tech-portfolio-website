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

        private readonly PortfolioDbContext _context;

        public ReminderController(PortfolioDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("GetAllReminderList")]
        public IActionResult GetAllReminderList()
        {
            return Ok(_context.Reminders.ToList());
        }

        [Authorize]
        [HttpPost("AddReminder")]
        public IActionResult AddReminder(Reminder reminder)
        {
            try
            {
                if (reminder == null)
                    return BadRequest("Reminder form data is required.");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _context.Reminders.Add(reminder);
                _context.SaveChanges();
                return Ok(reminder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error ex:" + ex.Message);
            }
        }
    }
}
