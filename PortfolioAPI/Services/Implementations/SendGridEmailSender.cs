using PortfolioAPI.Services.Contracts;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace PortfolioAPI.Services.Implementations
{
    public class SendGridEmailSender : IEmailSender
    {

        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _senderName;
        private readonly IHostEnvironment _env;

        public SendGridEmailSender(IConfiguration config , IHostEnvironment env)
        {
            _apiKey = config["SendGrid:ApiKey"];
            _senderEmail = config["SendGrid:SenderEmail"];
            _senderName = config["SendGrid:SenderName"];
            _env = env; 
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string userName)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_senderEmail))
                {
                    throw new ArgumentException("SendGrid configuration is invalid.");
                }


                // Load HTML template
                var templatePath = Path.Combine(_env.ContentRootPath, "Resources", "EmailTemplates", "ThankYouTemplate.html");
                if (!File.Exists(templatePath))
                    throw new FileNotFoundException("Email template not found at path:", templatePath);

                string htmlBody = await File.ReadAllTextAsync(templatePath);
                
                //Replace placeholders if needed
                htmlBody = htmlBody.Replace("##username##", userName);


                var client = new SendGridClient(_apiKey);
                var from = new SendGrid.Helpers.Mail.EmailAddress(_senderEmail, _senderName);
                var to = new SendGrid.Helpers.Mail.EmailAddress(toEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: "Thank you for contacting me.", htmlContent: htmlBody);

                var response = await client.SendEmailAsync(msg);

                if ((int)response.StatusCode >= 400)
                {
                    var errorDetails = await response.Body.ReadAsStringAsync();
                    throw new Exception($"SendGrid email failed with status {(int)response.StatusCode}: {errorDetails}");
                }

                Console.WriteLine($"Email sent to {toEmail}");
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine($"Configuration error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error sending email: {ex.Message}");
            }
        }
    }
}
