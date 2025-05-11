using PortfolioAPI.Services.Contracts;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Logging;

namespace PortfolioAPI.Services.Implementations
{
    public class TwilioSmsSender : ISmsSender
    {
        private readonly IConfiguration _config;
        private readonly ILogger<TwilioSmsSender> _logger;

        public TwilioSmsSender(IConfiguration config, ILogger<TwilioSmsSender> logger)
        {
            _config = config;
            _logger = logger;

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

                if (string.IsNullOrEmpty(fromPhone) || string.IsNullOrEmpty(toPhoneNumber))
                {
                    _logger.LogError("FromPhone or ToPhoneNumber is null or empty. From: '{From}', To: '{To}'", fromPhone, toPhoneNumber);
                    throw new ArgumentException("Phone number cannot be null or empty.");
                }

                var result = await MessageResource.CreateAsync(
                    body: message,
                    from: new Twilio.Types.PhoneNumber(fromPhone),
                    to: new Twilio.Types.PhoneNumber(toPhoneNumber)
                );

                _logger.LogInformation("SMS sent successfully. SID: {Sid}", result.Sid);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument provided: {Message}", ex.Message);
            }
            catch (Twilio.Exceptions.ApiException ex)
            {
                _logger.LogError(ex, "Twilio API error: {Message}", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unexpected error occurred while sending SMS: {Message}", ex.Message);
            }
        }
    }
}
