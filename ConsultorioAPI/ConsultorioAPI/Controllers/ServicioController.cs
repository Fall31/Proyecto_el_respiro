using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicioController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicioDto>>> GetAll()
        {
            var servicios = await _context.Servicios
                .AsNoTracking()
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Precio = s.Precio,
                    DuracionMinutos = s.DuracionMinutos,
                    Activo = s.Activo
                })
                .ToListAsync();

            return Ok(servicios);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ServicioDto>> Get(int id)
        {
            var dto = await _context.Servicios
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new ServicioDto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Precio = s.Precio,
                    DuracionMinutos = s.DuracionMinutos,
                    Activo = s.Activo
                })
                .FirstOrDefaultAsync();

            if (dto == null) return NotFound();
            return Ok(dto);
        }
        [HttpPost]
        public async Task<ActionResult<ServicioDto>> Create([FromBody] CreateServicioDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var s = new Servicio
            {
                Nombre = dto.Nombre,
                Precio = dto.Precio,
                DuracionMinutos = dto.DuracionMinutos,
                Activo = dto.Activo
            };

            _context.Servicios.Add(s);
            await _context.SaveChangesAsync();

            var resultDto = new ServicioDto
            {
                Id = s.Id,
                Nombre = s.Nombre,
                Precio = s.Precio,
                DuracionMinutos = s.DuracionMinutos,
                Activo = s.Activo
            };

            return CreatedAtAction(nameof(Get), new { id = s.Id }, resultDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchServicioDto dto)
        {
            if (dto == null) return BadRequest();

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return NotFound();

            TryValidateModel(dto);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.Nombre != null) servicio.Nombre = dto.Nombre;
            if (dto.Precio.HasValue) servicio.Precio = dto.Precio.Value;
            if (dto.DuracionMinutos.HasValue) servicio.DuracionMinutos = dto.DuracionMinutos.Value;
            if (dto.Activo.HasValue) servicio.Activo = dto.Activo.Value;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _context.Servicios.FindAsync(id);
            if (s == null) return NotFound();

            bool tieneTurnos = await _context.Turnos.AnyAsync(t => t.ServicioId == id);
            if (tieneTurnos)
                return BadRequest(new { message = "Este servicio está asociado a turnos y no puede eliminarse." });

            _context.Servicios.Remove(s);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

