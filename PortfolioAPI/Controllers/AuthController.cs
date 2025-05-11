// Controllers/AuthController.cs
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PortfolioAPI.Models;
using PortfolioAPI.Services;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly JwtKeyManagerService _jwtKeyManagerService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration config, JwtKeyManagerService jwtKeyManagerService, ILogger<AuthController> logger)
    {
        _configuration = config;
        _jwtKeyManagerService = jwtKeyManagerService;
        _logger = logger;
    }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginModel model)
    {
        try
        {
            if (model == null)
            {
                _logger.LogError("AdminLogin failed: Request body is null.");
                return BadRequest("Request body is required.");
            }

            var clientKey = _configuration["Encryption:ClientKey"]!;
            var keyBase64 = _configuration["Encryption:keyBase64"]!;
            var ivBase64 = _configuration["Encryption:ivBase64"]!;

            // Validate if necessary configuration values exist
            if (string.IsNullOrEmpty(clientKey) || string.IsNullOrEmpty(keyBase64) ||
                string.IsNullOrEmpty(ivBase64) || string.IsNullOrEmpty(_configuration["AdminInfo:UserId"]) ||
                string.IsNullOrEmpty(_configuration["AdminInfo:Password"]))
            {
                _logger.LogError("AdminLogin failed: Missing configuration values.");
                return StatusCode(500, "Internal server error: Missing configuration values.");
            }

            string decryptedPassword = AesEncryption.DecryptClientKey(model.Password, keyBase64, ivBase64);

            if (model.Email == _configuration["AdminInfo:UserId"] && decryptedPassword == _configuration["AdminInfo:Password"])
            {
                var keyVersion = DateTime.UtcNow.ToString("yyyyMMddHH"); // hourly version
                var signingKey = await _jwtKeyManagerService.GetKeyAsync(keyVersion);
                var keyBytes = Encoding.UTF8.GetBytes(signingKey);

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, "Admin")
            }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("AdminLogin succeeded for user: {UserEmail} at {Timestamp}", model.Email, DateTime.UtcNow);

                return Ok(new { token = tokenHandler.WriteToken(token) });
            }
            _logger.LogWarning("AdminLogin failed: Invalid credentials for user: {UserEmail}", model.Email);
            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during admin login for user: {UserEmail}", model.Email);
            return BadRequest(ex.Message);
        }
    }

}
