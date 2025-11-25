using ConsultorioAPI.Data;
using ConsultorioAPI.DTOs;
using ConsultorioAPI.Models;
using Google.Apis.Auth; // La librería que instalamos
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // Para crear JWT
using System.IdentityModel.Tokens.Jwt; // Para crear JWT
using System.Security.Claims; // Para los datos del usuario
using System.Text;

namespace ConsultorioAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> GoogleLoginAsync(GoogleLoginDto dto)
        {
            // 1. VERIFICAR CON GOOGLE (El interrogatorio)
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential);
            }
            catch
            {
                throw new Exception("El token de Google es falso o expiró. ¡Alerta de seguridad!");
            }

            // 2. BUSCAR EN NUESTRA BASE DE DATOS
            // Buscamos un usuario que tenga ese email
            var usuario = await _context.Usuarios
                .Include(u => u.Rol) // Importante: Traer el Rol para saber si es Admin
                .FirstOrDefaultAsync(u => u.Email == payload.Email);

            // 3. SI NO EXISTE, LO REGISTRAMOS (Auto-Register)
            if (usuario == null)
            {
                // Buscamos el ID del rol "Cliente" (asumimos que existe por el Seed)
                var rolCliente = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
                if (rolCliente == null) throw new Exception("Error fatal: No existe el rol Cliente en la BD.");

                usuario = new Usuario
                {
                    Nombre = payload.Name ?? "Usuario Sin Nombre", // A veces Google no manda nombre
                    Email = payload.Email,
                    GoogleSubjectId = payload.Subject, // El ID único de Google
                    RolId = rolCliente.Id,
                    Rol = rolCliente,
                    FechaRegistro = DateTime.UtcNow
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
            }

            // 4. GENERAR NUESTRO TOKEN JWT (El Pasaporte)
            var token = GenerarTokenJwt(usuario);

            // 5. DEVOLVER LA RESPUESTA
            return new AuthResponseDto
            {
                Token = token,
                NombreUsuario = usuario.Nombre,
                Rol = usuario.Rol!.Nombre // "Admin" o "Cliente"
            };
        }

        // --- MÉTODO PRIVADO: LA FÁBRICA DE TOKENS ---
        private string GenerarTokenJwt(Usuario usuario)
        {
            // A. Crear los "Claims" (Datos que van pegados en el token)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()), // ID del usuario
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),       // Email
                new Claim(ClaimTypes.Role, usuario.Rol!.Nombre),               // Rol (CRUCIAL para los permisos)
                new Claim("Nombre", usuario.Nombre)                            // Dato extra
            };

            // B. Obtener la clave secreta del appsettings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // C. Configurar el token
            var token = new JwtSecurityToken(
                issuer: null, // Lo dejamos null para simplificar dev
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(7), // El token dura 1 semana
                signingCredentials: creds
            );

            // D. Escribirlo como string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
