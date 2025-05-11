using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PortfolioAPI.Data;
using PortfolioAPI.HostedServices;
using PortfolioAPI.Services.Contracts;
using PortfolioAPI.Services.Implementations;
using Serilog.Events;
using Serilog;
using System;
using System.Text;
using Serilog.Exceptions;

namespace PortfolioAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File($"Logs/log-{env}-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj} {Exception}{NewLine}")
                .CreateLogger();

            builder.Host.UseSerilog();

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
                string crosOrginURL = "";
                if (env == "Production")
                {
                    crosOrginURL = builder.Configuration["FrontendURLProd"]!;
                }
                else
                {
                    crosOrginURL =  builder.Configuration["FrontendURL"]!;
                }
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(crosOrginURL)
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

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Description = "Enter 'Bearer' followed by your token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                         }
                    },
                    new string[] {}
                }
                });
            });


            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();
            builder.Services.AddSingleton<ISmsSender, TwilioSmsSender>();

            var app = builder.Build();

            // Dynamically configure JwtBearer options after DI container is built
            using (var scope = app.Services.CreateScope())
            {
                var keyManager = scope.ServiceProvider.GetRequiredService<JwtKeyManagerService>();
                string currentVersion = DateTime.UtcNow.ToString("yyyyMMddHH");
                var signingKey = keyManager.GetKeyAsync(currentVersion).GetAwaiter().GetResult();

                if (!string.IsNullOrEmpty(signingKey))
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
