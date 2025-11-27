using ConsultorioAPI.Data;
using ConsultorioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsultorioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pago>>> GetAll()
        {
            var pagos = await _context.Pagos.AsNoTracking().ToListAsync();
            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pago>> Get(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();
            return pago;
        }

        [HttpPost]
        public async Task<ActionResult<Pago>> Create([FromBody] Pago pago)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validate Turno exists
            var turno = await _context.Turnos.FindAsync(pago.TurnoId);
            if (turno == null) return BadRequest(new { message = "Turno inválido." });

            // Ensure a pago for this turno does not already exist (one-to-one)
            var exists = await _context.Pagos.AnyAsync(p => p.TurnoId == pago.TurnoId);
            if (exists) return BadRequest(new { message = "El turno ya tiene un pago asociado." });

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = pago.Id }, pago);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Pago pago)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != pago.Id) return BadRequest(new { message = "Id del pago no coincide con la ruta." });

            var existing = await _context.Pagos.FindAsync(id);
            if (existing == null) return NotFound();

            // If TurnoId changed, validate target turno and uniqueness
            if (existing.TurnoId != pago.TurnoId)
            {
                var turno = await _context.Turnos.FindAsync(pago.TurnoId);
                if (turno == null) return BadRequest(new { message = "Turno inválido." });

                var exists = await _context.Pagos.AnyAsync(p => p.TurnoId == pago.TurnoId && p.Id != id);
                if (exists) return BadRequest(new { message = "El turno ya tiene un pago asociado." });
            }

            existing.Monto = pago.Monto;
            existing.MetodoPago = pago.MetodoPago;
            existing.FechaPago = pago.FechaPago;
            existing.ComprobanteUrl = pago.ComprobanteUrl;
            existing.TurnoId = pago.TurnoId;

            _context.Pagos.Update(existing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
