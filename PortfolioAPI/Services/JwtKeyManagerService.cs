using PortfolioAPI.Models;
using System.Security.Cryptography;
using System;
using PortfolioAPI.Data;
using Microsoft.EntityFrameworkCore;

public class JwtKeyManagerService
{
    private readonly PortfolioDbContext _context;
    private readonly IConfiguration _configuration;

    public JwtKeyManagerService(PortfolioDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> GetKeyAsync(string version)
    {
        var existingKey = await _context.JwtKeys.FirstOrDefaultAsync(k => k.KeyVersion == version);
        if (existingKey != null)
        {
            return AesEncryption.Decrypt(existingKey.EncryptedKey, _configuration["Encryption:MasterKey"]!);
        }

        var key = GenerateSecureKey();
        var encryptedKey = AesEncryption.Encrypt(key, _configuration["Encryption:MasterKey"]!);
        var newKey = new JwtKey
        {
            KeyVersion = version,
            EncryptedKey = encryptedKey,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };

        _context.JwtKeys.Add(newKey);
        await _context.SaveChangesAsync();
        return key;
    }

    private string GenerateSecureKey()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
