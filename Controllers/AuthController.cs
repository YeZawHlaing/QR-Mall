using Microsoft.AspNetCore.Mvc;
using ProductQrApi.DTOs;
using ProductQrApi.Services;


namespace ProductQrApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // SIMPLE HARDCODED ADMIN (replace later with DB)
        if (dto.Username == "admin" && dto.Password == "1234")
        {
            var token = _jwtService.GenerateToken(dto.Username);

            return Ok(new
            {
                token
            });
        }

        return Unauthorized();
    }
}