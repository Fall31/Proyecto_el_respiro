using ConsultorioAPI.DTOs;

namespace ConsultorioAPI.Services
{
    public interface IAuthService
    {

        Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginDto dto);
    }
}