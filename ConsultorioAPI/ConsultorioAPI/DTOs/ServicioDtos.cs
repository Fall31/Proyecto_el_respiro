using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.DTOs
{

    public class ServicioCreateDto
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000)] 
        public decimal Precio { get; set; }

        [Required]
        [Range(15, 120)] 
        public int DuracionMinutos { get; set; }
    }

    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
    }
}
