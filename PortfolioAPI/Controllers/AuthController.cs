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
    private readonly JwtKeyManagerService  _jwtKeyManagerService;

    public AuthController(IConfiguration config, JwtKeyManagerService jwtKeyManagerService)
    {
        _configuration = config;
        _jwtKeyManagerService = jwtKeyManagerService;
    }

    [HttpPost("admin-login")]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginModel model)
    {
        try
        {

            var clientKey = _configuration["Encryption:ClientKey"]!;

            const string keyBase64 = "mZp9cUHQaYF0wAnJSF6xAx/9v2+/ZkBGNz4H1z5HezY=";
            const string ivBase64 = "MTIzNDU2Nzg5MGFiY2RlZg==";

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
                return Ok(new { token = tokenHandler.WriteToken(token) });
            }

            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
