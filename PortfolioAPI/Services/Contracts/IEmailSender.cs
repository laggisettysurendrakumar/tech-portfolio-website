namespace PortfolioAPI.Services.Contracts
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string body, string userName);
    }
}
