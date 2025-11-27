using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ConsultorioAPI.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty; 

        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}