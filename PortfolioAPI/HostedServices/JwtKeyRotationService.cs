using PortfolioAPI.Data;
using System;

namespace PortfolioAPI.HostedServices
{

    public class JwtKeyRotationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public JwtKeyRotationService(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
                var keyManager = new JwtKeyManagerService(dbContext, _configuration);

                try
                {
                    // Pre-generate next hour's key
                    var nextHourVersion = DateTime.UtcNow.AddHours(1).ToString("yyyyMMddHH");
                    await keyManager.GetKeyAsync(nextHourVersion);

                    // Optionally, clean up keys older than 2 hours
                    var expirationThreshold = DateTime.UtcNow.AddHours(-2);
                    var oldKeys = dbContext.JwtKeys.Where(k => k.ExpiresAt < expirationThreshold);
                    dbContext.JwtKeys.RemoveRange(oldKeys);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    // Log the exception if needed
                    Console.WriteLine($"[JwtKeyRotationService] Error: {ex.Message}");
                }

                // Wait for 10 minutes before checking again
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}