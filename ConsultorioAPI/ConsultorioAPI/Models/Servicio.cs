using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Models
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; 

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int DuracionMinutos { get; set; } 

        public bool Activo { get; set; } = true; 
    }
}