using ConsultorioAPI.Data;
using ConsultorioAPI.Models;
using ConsultorioAPI.Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

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

        private static PagoDto MapToDto(Pago pago)
        {
            return new PagoDto
            {
                Id = pago.Id,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago,
                FechaPago = pago.FechaPago
            };
        }

        private static Pago MapFromCreateDto(CreatePagoDto dto)
        {
            return new Pago
            {
                Monto = dto.Monto,
                MetodoPago = dto.MetodoPago,
                FechaPago = dto.FechaPago ?? DateTime.UtcNow
            };
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetAll()
        {
            var pagos = await _context.Pagos.AsNoTracking().ToListAsync();
            var dtos = pagos.Select(MapToDto).ToList();
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDto>> Get(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();
            return MapToDto(pago);
        }

        [HttpPost]
        public async Task<ActionResult<PagoDto>> Create([FromBody] CreatePagoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pago = MapFromCreateDto(dto);
            pago.Id = 0;

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            var resultDto = MapToDto(pago);
            return CreatedAtAction(nameof(Get), new { id = pago.Id }, resultDto);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] PatchPagoDto dto)
        {
            if (dto == null) return BadRequest();

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            TryValidateModel(dto);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (dto.Monto.HasValue) pago.Monto = dto.Monto.Value;
            if (dto.MetodoPago != null) pago.MetodoPago = dto.MetodoPago;
            if (dto.FechaPago.HasValue) pago.FechaPago = dto.FechaPago.Value;

            _context.Pagos.Update(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.PagoId == id);
            if (turno != null)
            {
                turno.PagoId = null;
                _context.Turnos.Update(turno);
                await _context.SaveChangesAsync();
            }

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
