using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Dtos;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IUserService userService, 
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _userService = userService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
    {
        try
        {
            var existingUser = _userService.FindUserByEmail(model.Email);
            if (existingUser != null)
                return Conflict("Пользователь с таким email уже существует");

            
            var userId = _userService.PutUser(model.Email, model.Password);
            
            return Ok(new { UserId = userId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при регистрации пользователя");
            return StatusCode(500, "Произошла ошибка при регистрации");
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto user)
    {
        try
        {
            var existedUser = _userService.FindUserByEmail(user.Email);
            if (existedUser == null)
                return Unauthorized("Неверный email или пароль");

            if (existedUser.Password != user.Password)
                return Unauthorized("Неверный email или пароль");

            var token = GenerateJwtToken(existedUser);

            return Ok(new
            {
                Token = token,
                UserId = existedUser.Id,
                Email = existedUser.Email
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при входе пользователя");
            return StatusCode(500, "Произошла ошибка при входе");
        }
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
