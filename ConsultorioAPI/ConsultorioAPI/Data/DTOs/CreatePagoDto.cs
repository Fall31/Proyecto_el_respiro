using System;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class CreatePagoDto
    {
        [Required]
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty;

        // Opcional: si no se proporciona, el servidor puede asignar DateTime.UtcNow
        public DateTime? FechaPago { get; set; }

        [MaxLength(250)]
        public string? ComprobanteUrl { get; set; }

        [Required]
        public int TurnoId { get; set; }
    }
}
