using System;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class PatchTurnoDto
    {
        public DateTime? FechaHoraInicio { get; set; }
        public DateTime? FechaHoraFin { get; set; }

        [MaxLength(20)]
        public string? Estado { get; set; }

        public string? Notas { get; set; }

        public int? UsuarioId { get; set; }
        public int? ServicioId { get; set; }
        public int? PagoId { get; set; }
    }
}
