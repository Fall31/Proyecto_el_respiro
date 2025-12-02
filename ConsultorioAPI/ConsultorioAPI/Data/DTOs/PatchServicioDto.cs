using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class PatchServicioDto
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal? Precio { get; set; }

        [Range(1, 1440, ErrorMessage = "Duración inválida.")]
        public int? DuracionMinutos { get; set; }

        public bool? Activo { get; set; }
    }
}
