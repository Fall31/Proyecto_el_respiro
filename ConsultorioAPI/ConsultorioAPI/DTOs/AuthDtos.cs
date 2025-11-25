using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.DTOs
{

    public class GoogleLoginDto
    {
        [Required]
        public string Credential { get; set; } = string.Empty; // El token largo de Google
    }


    public class RegisterUserDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }


    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty; 
        public string NombreUsuario { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty; 
    }
}
