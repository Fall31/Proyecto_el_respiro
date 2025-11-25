using ConsultorioAPI.DTOs;
using ConsultorioAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConsultorioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/google
        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            try
            {
                var response = await _authService.GoogleLoginAsync(dto);
                return Ok(response); // Devuelve 200 OK con el Token
            }
            catch (Exception ex)
            {
                // Si algo falla (token falso, error de BD), devolvemos 400 Bad Request
                return BadRequest(new { mensaje = ex.Message });
            }
        }
    }
}