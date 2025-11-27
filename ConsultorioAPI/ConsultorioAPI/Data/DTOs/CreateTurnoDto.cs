using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class CreateTurnoDto
    {
        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public DateTime FechaHoraFin { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = string.Empty;

        public string? Notas { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int ServicioId { get; set; }

        public int? PagoId { get; set; }
    }
}
