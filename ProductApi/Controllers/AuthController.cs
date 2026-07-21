using Application.DTOs;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthController(
            IJwtService jwtService,
            ApplicationDbContext context)
        {
            _jwtService = jwtService;
            _context = context;
        }
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var existingToken = _context.RefreshTokens
                .FirstOrDefault(x =>
                    x.Token == request.RefreshToken &&
                    !x.IsRevoked &&
                    x.Expires > DateTime.UtcNow);

            if (existingToken == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid or expired refresh token."
                });
            }

           
            existingToken.IsRevoked = true;

            var accessToken = _jwtService.GenerateToken(existingToken.Username, "Admin");
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Username = existingToken.Username,
                Token = newRefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges();

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                TokenType = "Bearer"
            });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            
            if (loginDto.Username != "admin" || loginDto.Password != "Admin@123")
            {
                return Unauthorized(new
                {
                    Message = "Invalid username or password."
                });
            }

            var accessToken = _jwtService.GenerateToken(loginDto.Username, "Admin");
            var refreshToken = _jwtService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Username = loginDto.Username,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges();

            return Ok(new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenType = "Bearer"
            });
        }
    }
}


