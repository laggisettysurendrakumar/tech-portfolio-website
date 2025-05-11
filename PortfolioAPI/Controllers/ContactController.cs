using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models.DTO;
using PortfolioAPI.Services.Contracts;

namespace PortfolioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {

        private readonly IContactService _contactService;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService contactService, IEmailSender emailSender, ISmsSender smsSender, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = logger;
        }


        [HttpPost]
        public async Task<IActionResult> PostContactForm([FromBody] ContactFormDto contactFormDto)
        {
            if (contactFormDto == null)
            {
                _logger.LogError("Contact form submission failed: request body was null.");
                return BadRequest("Contact form data is required.");
            }

            _logger.LogInformation("Processing contact form submission for user: {UserEmail}", contactFormDto.Email);

            try
            {
                // Attempt to save the contact form
                var saved = await _contactService.SaveContactFormAsync(contactFormDto);

                if (saved == null)
                {
                    _logger.LogWarning("Contact form submission failed: {UserEmail} submission could not be saved.", contactFormDto.Email);
                    return BadRequest("Failed to save the contact form.");
                }

                // Send email and SMS notifications
                var message = $"From: {contactFormDto.Name} ({contactFormDto.Email})\n\n{contactFormDto.Message}";
                await _emailSender.SendEmailAsync(contactFormDto.Email, "Contact Form Submission", message, contactFormDto.Name);
               // await _smsSender.SendSmsAsync("+918886079906", $"New message from {contactFormDto.Name}: {contactFormDto.Message}");

                _logger.LogInformation("Contact form submitted successfully by: {UserEmail} at {Timestamp}", contactFormDto.Email, DateTime.UtcNow);

                // Return Created status with saved entity ID and info
                return CreatedAtAction(nameof(GetContactById), new { id = saved.Id }, saved);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error while processing the contact form submission for user: {UserEmail}", contactFormDto.Email);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while processing the contact form submission for user: {UserEmail}", contactFormDto.Email);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int contactId)
        {
            if (contactId <= 0)
            {
                _logger.LogError("Invalid contactId {ContactId} received for {Method}", contactId, nameof(GetContactById));
                return BadRequest("Invalid Contact Id.");
            }

            try
            {
                _logger.LogInformation("{Method} invoked for ContactId: {ContactId}", nameof(GetContactById), contactId);

                var contact = await _contactService.GetContactInfoByID(contactId);

                if (contact == null)
                {
                    _logger.LogWarning("Contact not found for ContactId: {ContactId} in {Method}", contactId, nameof(GetContactById));
                    return NotFound("Contact not found.");
                }

                _logger.LogInformation("{Method} successfully retrieved contact for ContactId: {ContactId}", nameof(GetContactById), contactId);
                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing {Method} for ContactId: {ContactId}", nameof(GetContactById), contactId);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetContactSubmissionList")]
        public async Task<IActionResult> GetContactSubmissionList()
        {
            try
            {
                _logger.LogInformation("{Method} was invoked", nameof(GetContactSubmissionList));

                var contactSubmissionsList = await _contactService.GetContactSubmissionListAsync();

                // Early return on failure, simplifies code flow
                if (contactSubmissionsList == null || !contactSubmissionsList.Any())
                {
                    _logger.LogWarning("No contact submissions found for {Method}", nameof(GetContactSubmissionList));
                    return NotFound("Contact submissions not found.");
                }

                _logger.LogInformation("{Method} completed successfully with {Count} submissions", nameof(GetContactSubmissionList), contactSubmissionsList.Count());
                return Ok(contactSubmissionsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing {Method}", nameof(GetContactSubmissionList));
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
