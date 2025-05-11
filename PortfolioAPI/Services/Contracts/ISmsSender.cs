namespace PortfolioAPI.Services.Contracts
{
    public interface ISmsSender
    {
         Task SendSmsAsync(string toPhoneNumber, string message);
    }
}
