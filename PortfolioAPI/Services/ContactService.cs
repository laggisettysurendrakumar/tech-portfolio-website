using PortfolioAPI.Data;
using PortfolioAPI.Models.DTO;
using PortfolioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PortfolioAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly PortfolioDbContext _portfolioDbContext;

        public ContactService(PortfolioDbContext portfolioDbContext)
        {
            _portfolioDbContext = portfolioDbContext;
        }

        public async Task<ContactFormModel> GetContactInfoByID(int contactId)
        {

            var contact = await _portfolioDbContext.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);

            if (contact == null)
            {
                throw new Exception("Contact not found");
            }

            return contact;
        }

        public async Task<List<ContactFormModel>> GetContactSubmissionListAsync()
        {
            return await _portfolioDbContext.Contacts.ToListAsync();
        }


        public async Task<ContactFormModel> SaveContactFormAsync(ContactFormDto contact)
        {
            // Basic validation (you can improve with FluentValidation or Data Annotations)  
            if (string.IsNullOrWhiteSpace(contact.Name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(contact.Email) || !contact.Email.Contains("@"))
                throw new ArgumentException("Valid Email is required.");

            if (string.IsNullOrWhiteSpace(contact.Message))
                throw new ArgumentException("Message is required.");

            var entity = new ContactFormModel
            {
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                SubmittedAt = DateTime.UtcNow
            };

            _portfolioDbContext.Contacts.Add(entity);
            await _portfolioDbContext.SaveChangesAsync();

            return entity;
        }
    }
}
