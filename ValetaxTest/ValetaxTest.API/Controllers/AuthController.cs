using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ValetaxTest.Application.Requests;
using ValetaxTest.Application.Responses;
using ValetaxTest.API.Services;

namespace ValetaxTest.API.Controllers;

[ApiController]
[Route("api.user.partner")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserService _userService;

    public AuthController(
        IConfiguration configuration,
        UserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    [HttpPost("rememberMe")]
    public IActionResult RememberMe([FromBody] LoginRequest request)  // <-- Изменил на FromBody
    {
        // Валидация
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required");
        }

        // Проверяем пользователя
        if (!_userService.ValidateUser(request.Username, request.Password))
        {
            return Unauthorized("Invalid username or password");
        }

        // Генерируем токен
        var token = GenerateJwtToken(request.Username);

        return Ok(new TokenResponse { Token = token });
    }

    private string GenerateJwtToken(string username)
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ??
            "awert-qwert-qwe-qwe-qwer-qwer-qwert");
        var issuer = _configuration["Jwt:Issuer"] ?? "ValetaxTest";
        var audience = _configuration["Jwt:Audience"] ?? "ValetaxTestClient";

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, username),
            new Claim("username", username)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}