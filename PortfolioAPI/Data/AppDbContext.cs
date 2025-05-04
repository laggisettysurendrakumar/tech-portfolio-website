using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Models;
using System.Collections.Generic;

namespace PortfolioAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ContactFormModel> Contacts { get; set; }

        // Optional: Seed data or configure via Fluent API here  
    }
}
