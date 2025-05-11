using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models.DTO;
using PortfolioAPI.Services;

namespace PortfolioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpPost]
        public async Task<IActionResult> PostContactForm([FromBody] ContactFormDto contactFormDto)
        {

            if (contactFormDto == null)
                return BadRequest("Contact form data is required.");

            try
            {
                var saved = await _contactService.SaveContactFormAsync(contactFormDto);
                // Return Created status with saved entity ID and info  
                return CreatedAtAction(nameof(GetContactById), new { id = saved.Id }, saved);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            if (contactId == 0)
                return BadRequest("Invalid Contact Id.");

            try
            {
                var contact = await _contactService.GetContactInfoByID(contactId);

                if (contact == null)
                    return NotFound("Contact not found.");

                return Ok(contact);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error ex:"+ ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetContactSubmissionList")]
        public async Task<IActionResult> GetContactSubmissionList()
        {
            
            try
            {
                var contactSubmistionsList = await _contactService.GetContactSubmissionListAsync();

                if (contactSubmistionsList == null)
                    return NotFound("Contact not found.");

                return Ok(contactSubmistionsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error ex:" + ex.Message);
            }

        }
    }
}
