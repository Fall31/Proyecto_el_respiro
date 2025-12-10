using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Services;
using ConsultorioAPI.Models;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return Unauthorized(new { message = "Credenciales inválidas" });

            if (user.PasswordHash != dto.Password) return Unauthorized(new { message = "Credenciales inválidas" });

            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshTokenString = _jwtService.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenString,
                UsuarioId = user.Id,
                Expiration = System.DateTime.UtcNow.AddDays(int.Parse(_jwtService.GetType().GetProperty("_refreshTokenDays", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_jwtService)?.ToString() ?? "7"))
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenString,
                ExpiresAt = System.DateTime.UtcNow.AddMinutes(int.Parse(_jwtService.GetType().GetProperty("_accessTokenMinutes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_jwtService)?.ToString() ?? "15")),
                UserId = user.Id,
                Role = user.Rol?.Nombre ?? string.Empty
            };

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponseDto>> Refresh([FromBody] RefreshRequestDto dto)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(dto.AccessToken);
            if (principal == null) return BadRequest(new { message = "Token inválido" });

            var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return BadRequest(new { message = "Token inválido" });

            if (!int.TryParse(userIdClaim.Value, out var userId)) return BadRequest(new { message = "Token inválido" });

            var stored = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && r.UsuarioId == userId);
            if (stored == null || stored.IsExpired) return Unauthorized(new { message = "Refresh token inválido o expirado" });

            var user = await _context.Usuarios.Include(u => u.Rol).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return Unauthorized();

            var newAccess = _jwtService.GenerateAccessToken(user);
            var newRefresh = _jwtService.GenerateRefreshToken();

  
            stored.Token = newRefresh;
            stored.Expiration = System.DateTime.UtcNow.AddDays(int.Parse(_jwtService.GetType().GetProperty("_refreshTokenDays", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_jwtService)?.ToString() ?? "7"));
            _context.RefreshTokens.Update(stored);
            await _context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = newAccess,
                RefreshToken = newRefresh,
                ExpiresAt = System.DateTime.UtcNow.AddMinutes(int.Parse(_jwtService.GetType().GetProperty("_accessTokenMinutes", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_jwtService)?.ToString() ?? "15")),
                UserId = user.Id,
                Role = user.Rol?.Nombre ?? string.Empty
            };

            return Ok(response);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RefreshRequestDto dto)
        {
            var stored = await _context.RefreshTokens.FirstOrDefaultAsync(r => r.Token == dto.RefreshToken);
            if (stored == null) return NotFound();

            _context.RefreshTokens.Remove(stored);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
