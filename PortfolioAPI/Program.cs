using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PortfolioAPI.Data;
using PortfolioAPI.HostedServices;
using PortfolioAPI.Services;
using System;
using System.Text;

namespace PortfolioAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // EF Core SQLite 
            builder.Services.AddDbContext<PortfolioDbContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // Add custom services
            builder.Services.AddScoped<IContactService, ContactService>();
            builder.Services.AddScoped<JwtKeyManagerService>();
            builder.Services.AddHostedService<JwtKeyRotationService>();

            // CORS Configuration
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(builder.Configuration["FrontendURLProd"]!)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // JWT Configuration (with post-build registration to resolve async dependencies)
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(); // Configure options below, after Build()

            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Dynamically configure JwtBearer options after DI container is built
            using (var scope = app.Services.CreateScope())
            {
                var keyManager = scope.ServiceProvider.GetRequiredService<JwtKeyManagerService>();
                string currentVersion = DateTime.UtcNow.ToString("yyyyMMddHH");
                var signingKey = keyManager.GetKeyAsync(currentVersion).GetAwaiter().GetResult();

                if (!signingKey.IsNullOrEmpty())
                {
                    var jwtBearerOptions = app.Services.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
                    jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                    };
                }
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
