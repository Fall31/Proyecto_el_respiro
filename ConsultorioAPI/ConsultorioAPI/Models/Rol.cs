using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; 

        public ICollection<Usuario>? Usuarios { get; set; }
    }
}