using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.DTOs
{

    public class CrearTurnoDto
    {
        [Required]
        public int ServicioId { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        // ¡OJO! No pedimos UsuarioId. 
        // Eso lo sacamos del Token en el Backend por seguridad.
        // Tampoco pedimos FechaFin, la calculamos nosotros.
    }


    public class TurnoDto
    {
        public int Id { get; set; }
        public string NombreServicio { get; set; } = string.Empty;
        public string NombrePaciente { get; set; } = string.Empty; 
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal Precio { get; set; } 
    }
}
