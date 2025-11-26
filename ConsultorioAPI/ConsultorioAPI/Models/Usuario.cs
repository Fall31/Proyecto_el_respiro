using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsultorioAPI.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty; 

        public string? PasswordHash { get; set; }

        [Required]
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public ICollection<Turno>? Turnos { get; set; }
    }
}
