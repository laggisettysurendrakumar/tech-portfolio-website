using PortfolioAPI.Models.DTO;
using PortfolioAPI.Models;

namespace PortfolioAPI.Services.Contracts
{
    public interface IContactService
    {
        Task<ContactFormModel> SaveContactFormAsync(ContactFormDto contact);
        Task<ContactFormModel> GetContactInfoByID(int contactId);
        Task<List<ContactFormModel>> GetContactSubmissionListAsync();
    }
}