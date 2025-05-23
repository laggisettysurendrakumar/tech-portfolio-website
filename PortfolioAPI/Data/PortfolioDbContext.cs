﻿using Microsoft.EntityFrameworkCore;
using PortfolioAPI.Models;
using System.Collections.Generic;

namespace PortfolioAPI.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : base(options) { }

        public DbSet<ContactFormModel> Contacts { get; set; }

        public DbSet<JwtKey> JwtKeys { get; set; } = default!;

        public DbSet<Reminder> Reminders { get; set; }

        public DbSet<Feedback> Feedbacks => Set<Feedback>();

        // Optional: Seed data or configure via Fluent API here  
    }
}
