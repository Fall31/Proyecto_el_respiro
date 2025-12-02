using System;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class PatchPagoDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0.")]
        public decimal? Monto { get; set; }

        [MaxLength(50)]
        public string? MetodoPago { get; set; }

        public DateTime? FechaPago { get; set; }
    }
}
