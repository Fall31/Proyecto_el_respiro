using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Data.DTOs
{
    public class CreateRolDto
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }
}
