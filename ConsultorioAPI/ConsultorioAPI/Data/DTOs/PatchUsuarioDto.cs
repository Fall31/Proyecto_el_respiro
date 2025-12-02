using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class PatchUsuarioDto
    {
        [MaxLength(100)]
        public string? Nombre { get; set; }

        [EmailAddress]
        [MaxLength(150)]
        public string? Email { get; set; }

        public string? Password { get; set; }

        public int? RolId { get; set; }
    }
}
