using System;

namespace ConsultorioAPI.Data.DTOs
{
    public class TurnoDto
    {
        public int Id { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Notas { get; set; }

        public int UsuarioId { get; set; }

        public int ServicioId { get; set; }

        public int? PagoId { get; set; }
    }
}
