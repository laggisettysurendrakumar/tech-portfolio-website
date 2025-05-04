using PortfolioAPI.Data;
using PortfolioAPI.Models.DTO;
using PortfolioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PortfolioAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _db;

        public ContactService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ContactFormModel> GetContactInfoByID(int contactId)
        {
            return await _db.Contacts.FirstOrDefaultAsync(contact => contact.Id == contactId);
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

            _db.Contacts.Add(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
