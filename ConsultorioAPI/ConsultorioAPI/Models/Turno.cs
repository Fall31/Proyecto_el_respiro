using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultorioAPI.Models
{
    public class Turno
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public DateTime FechaHoraFin { get; set; } 

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } 

        public string? Notas { get; set; } 

        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        public int ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public Servicio? Servicio { get; set; }


        public int? PagoId { get; set; }
        [ForeignKey("PagoId")]
        public Pago? Pago { get; set; }
    }
}