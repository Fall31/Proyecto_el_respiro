using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return usuario;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario([FromBody] CreateUsuarioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                return Conflict(new { message = "El email ya está registrado." });
            }

            var rol = await _context.Roles.FindAsync(dto.RolId);
            if (rol == null) return BadRequest(new { message = "Rol inválido." });

            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = dto.Password,
                RolId = dto.RolId,
                FechaRegistro = System.DateTime.UtcNow
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchUsuarioDto dto)
        {
            if (dto == null) return BadRequest();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            TryValidateModel(dto);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.Email != null && dto.Email != usuario.Email)
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email && u.Id != id))
                    return Conflict(new { message = "El email ya está registrado." });
                usuario.Email = dto.Email;
            }

            if (dto.Nombre != null) usuario.Nombre = dto.Nombre;
            if (dto.Password != null) usuario.PasswordHash = dto.Password; // Note: password hashing not implemented here
            if (dto.RolId.HasValue && dto.RolId.Value != usuario.RolId)
            {
                var rol = await _context.Roles.FindAsync(dto.RolId.Value);
                if (rol == null) return BadRequest(new { message = "Rol inválido." });
                usuario.RolId = dto.RolId.Value;
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UsuarioDto>> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            var rol = await _context.Roles.FindAsync(usuario.RolId);

            var dto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Rol = rol?.Nombre ?? string.Empty,
                FechaRegistro = usuario.FechaRegistro
            };

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return Ok(dto);
        }
    }
}
