using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultorioAPI.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string MetodoPago { get; set; } = string.Empty; 

        public DateTime FechaPago { get; set; } = DateTime.UtcNow;

        public string? ComprobanteUrl { get; set; } 

        [Required]
        public int TurnoId { get; set; }
        [ForeignKey("TurnoId")]
        public Turno? Turno { get; set; }
    }
}
