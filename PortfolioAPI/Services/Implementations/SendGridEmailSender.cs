using PortfolioAPI.Services.Contracts;
using SendGrid.Helpers.Mail;
using SendGrid;
using Microsoft.Extensions.Logging;

namespace PortfolioAPI.Services.Implementations
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly IHostEnvironment _env;
        private readonly ILogger<SendGridEmailSender> _logger;

        public SendGridEmailSender(IConfiguration config, IHostEnvironment env, ILogger<SendGridEmailSender> logger)
        {
            _apiKey = config["SendGrid:ApiKey"];
            _senderEmail = config["SendGrid:SenderEmail"];
            _senderName = config["SendGrid:SenderName"];
            _env = env;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string userName)
        {
            try
            {
                _logger.LogInformation("Preparing to send email to {Recipient}", toEmail);

                if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_senderEmail))
                {
                    var error = "SendGrid configuration is invalid.";
                    _logger.LogError(error);
                    throw new ArgumentException(error);
                }

                // Load HTML template
                var templatePath = Path.Combine(_env.ContentRootPath, "Resources", "EmailTemplates", "ThankYouTemplate.html");
                if (!File.Exists(templatePath))
                {
                    var error = $"Email template not found at path: {templatePath}";
                    _logger.LogError(error);
                    throw new FileNotFoundException(error);
                }

                string htmlBody = await File.ReadAllTextAsync(templatePath);
                htmlBody = htmlBody.Replace("##username##", userName);

                var client = new SendGridClient(_apiKey);
                var from = new EmailAddress(_senderEmail, _senderName);
                var to = new EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "Thank you for contacting me.", htmlContent: htmlBody);

                var response = await client.SendEmailAsync(msg);

                if ((int)response.StatusCode >= 400)
                {
                    var errorDetails = await response.Body.ReadAsStringAsync();
                    _logger.LogError("SendGrid email failed with status {StatusCode}: {Details}", response.StatusCode, errorDetails);
                    throw new Exception($"SendGrid email failed: {errorDetails}");
                }

                _logger.LogInformation("Email successfully sent to {Recipient}", toEmail);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Configuration error while sending email.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while sending email.");
            }
        }
    }
}
