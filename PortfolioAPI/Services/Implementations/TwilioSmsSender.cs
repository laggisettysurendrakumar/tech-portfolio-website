using PortfolioAPI.Services.Contracts;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PortfolioAPI.Services.Implementations
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly IConfiguration _config;

        public TwilioSmsSender(IConfiguration config)
        {
            _config = config;

            TwilioClient.Init(
                _config["Twilio:AccountSid"],
                _config["Twilio:AuthToken"]
            );
        }

        public async Task SendSmsAsync(string toPhoneNumber, string message)
        {
            try
            {
                var fromPhone = _config["Twilio:FromPhone"];

                // Ensure that the fromPhone and toPhoneNumber are valid
                if (string.IsNullOrEmpty(fromPhone) || string.IsNullOrEmpty(toPhoneNumber))
                {
                    throw new ArgumentException("Phone number cannot be null or empty.");
                }

                // Send SMS using Twilio API
                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromPhone),
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );

                // You can log the result or take further actions if needed
                Console.WriteLine($"Message sent: {result.Sid}");
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions
                Console.Error.WriteLine($"Argument error: {ex.Message}");
                // You can log this error to a logging framework like Serilog, NLog, etc.
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                // Handle Twilio-specific API exceptions
                Console.Error.WriteLine($"Twilio API error: {ex.Message}");
                // You can log this error to a logging framework
            }
            catch (Exception ex)
            {
                // Catch any other general exceptions
                Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
                // You can log this error to a logging framework
            }
        }

    }
}
