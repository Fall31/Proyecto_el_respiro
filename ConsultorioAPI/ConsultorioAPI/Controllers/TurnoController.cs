using ConsultorioAPI.Data;
using ConsultorioAPI.Data.DTOs;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
        public async Task<ActionResult<IEnumerable<TurnoDto>>> GetAll()
        {
            var turnos = await _context.Turnos
                .AsNoTracking()
                .Include(t => t.Usuario).ThenInclude(u => u.Rol)
                .Include(t => t.Servicio)
                .Include(t => t.Pago)
                .ToListAsync();

            var dtos = turnos.Select(t => new TurnoDto
            {
                Id = t.Id,
                FechaHoraInicio = t.FechaHoraInicio,
                FechaHoraFin = t.FechaHoraFin,
                Estado = t.Estado,
                Notas = t.Notas,
                UsuarioId = t.UsuarioId,
                ServicioId = t.ServicioId,
                PagoId = t.PagoId
            }).ToList();

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TurnoDto>> Get(int id)
        {
            var t = await _context.Turnos
                .Include(tu => tu.Usuario).ThenInclude(u => u.Rol)
                .Include(ts => ts.Servicio)
                .Include(tp => tp.Pago)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (t == null) return NotFound();

            var dto = new TurnoDto
            {
                Id = t.Id,
                FechaHoraInicio = t.FechaHoraInicio,
                FechaHoraFin = t.FechaHoraFin,
                Estado = t.Estado,
                Notas = t.Notas,
                UsuarioId = t.UsuarioId,
                ServicioId = t.ServicioId,
                PagoId = t.PagoId,
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<Turno>> Create([FromBody] CreateTurnoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);
            if (usuario == null) return BadRequest(new { message = "Usuario inválido." });

            var servicio = await _context.Servicios.FindAsync(dto.ServicioId);
            if (servicio == null) return BadRequest(new { message = "Servicio inválido." });

            int? pagoId = null;
            if (dto.PagoId.HasValue)
            {
                if (dto.PagoId.Value != 0)
                {
                    var pago = await _context.Pagos.FindAsync(dto.PagoId.Value);
                    if (pago == null) return BadRequest(new { message = "Pago inválido." });
                    pagoId = dto.PagoId.Value;
                }
                else
                {
                    pagoId = null; 
                }
            }

            var turno = new Turno
            {
                FechaHoraInicio = dto.FechaHoraInicio,
                FechaHoraFin = dto.FechaHoraFin,
                Estado = dto.Estado,
                Notas = dto.Notas,
                UsuarioId = dto.UsuarioId,
                ServicioId = dto.ServicioId,
                PagoId = pagoId
            };

            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = turno.Id }, turno);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchTurnoDto dto)
        {
            if (dto == null) return BadRequest();

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null) return NotFound();

            TryValidateModel(dto);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.UsuarioId.HasValue && dto.UsuarioId.Value != turno.UsuarioId)
            {
                var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId.Value);
                if (usuario == null) return BadRequest(new { message = "Usuario inválido." });
                turno.UsuarioId = dto.UsuarioId.Value;
            }

            if (dto.ServicioId.HasValue && dto.ServicioId.Value != turno.ServicioId)
            {
                var servicio = await _context.Servicios.FindAsync(dto.ServicioId.Value);
                if (servicio == null) return BadRequest(new { message = "Servicio inválido." });
                turno.ServicioId = dto.ServicioId.Value;
            }

            if (dto.PagoId.HasValue)
            {
                if (dto.PagoId.Value != 0)
                {
                    var pago = await _context.Pagos.FindAsync(dto.PagoId.Value);
                    if (pago == null) return BadRequest(new { message = "Pago inválido." });
                    turno.PagoId = dto.PagoId.Value;
                }
                else
                {
                    turno.PagoId = null;
                }
            }

            if (dto.FechaHoraInicio.HasValue) turno.FechaHoraInicio = dto.FechaHoraInicio.Value;
            if (dto.FechaHoraFin.HasValue) turno.FechaHoraFin = dto.FechaHoraFin.Value;
            if (dto.Estado != null) turno.Estado = dto.Estado;
            if (dto.Notas != null) turno.Notas = dto.Notas;

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
