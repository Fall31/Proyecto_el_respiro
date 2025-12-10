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

        public DateTime? FechaPago { get; set; }
    }
}
