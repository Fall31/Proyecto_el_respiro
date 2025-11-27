using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rol>> GetRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();
            return rol;
        }

        [HttpPost]
        public async Task<ActionResult<Rol>> CreateRol([FromBody] CreateRolDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rol = new Rol { Nombre = dto.Nombre };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRol), new { id = rol.Id }, rol);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] CreateRolDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();

            rol.Nombre = dto.Nombre;

            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null) return NotFound();

            var hasUsuarios = await _context.Usuarios.AnyAsync(u => u.RolId == id);
            if (hasUsuarios)
            {
                return BadRequest(new { message = "No se puede eliminar el rol porque tiene usuarios asociados." });
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
