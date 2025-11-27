using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurnoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TurnoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turno>>> GetAll()
        {
            var turnos = await _context.Turnos.AsNoTracking().ToListAsync();
            return Ok(turnos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> Get(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();
            return turno;
        }

        [HttpPost]
        public async Task<ActionResult<Turno>> Create([FromBody] CreateTurnoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validate related entities
            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return BadRequest(new { message = "Usuario inválido." });

            var servicio = await _context.Servicios.FindAsync(dto.ServicioId);
            if (servicio == null) return BadRequest(new { message = "Servicio inválido." });

            if (dto.PagoId.HasValue)
            {
                var pago = await _context.Pagos.FindAsync(dto.PagoId.Value);
                if (pago == null) return BadRequest(new { message = "Pago inválido." });
            }

            var turno = new Turno
            {
                FechaHoraInicio = dto.FechaHoraInicio,
                FechaHoraFin = dto.FechaHoraFin,
                Estado = dto.Estado,
                Notas = dto.Notas,
                UsuarioId = dto.UsuarioId,
                ServicioId = dto.ServicioId,
                PagoId = dto.PagoId ?? 0
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = turno.Id }, turno);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateTurnoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return BadRequest(new { message = "Usuario inválido." });

            var servicio = await _context.Servicios.FindAsync(dto.ServicioId);
            if (servicio == null) return BadRequest(new { message = "Servicio inválido." });

            if (dto.PagoId.HasValue)
            {
                var pago = await _context.Pagos.FindAsync(dto.PagoId.Value);
                if (pago == null) return BadRequest(new { message = "Pago inválido." });
            }

            turno.FechaHoraInicio = dto.FechaHoraInicio;
            turno.FechaHoraFin = dto.FechaHoraFin;
            turno.Estado = dto.Estado;
            turno.Notas = dto.Notas;
            turno.UsuarioId = dto.UsuarioId;
            turno.ServicioId = dto.ServicioId;
            turno.PagoId = dto.PagoId ?? turno.PagoId;

            _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
