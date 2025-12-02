using System;

namespace ConsultorioAPI.Data.DTOs
{
    public class PagoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public DateTime FechaPago { get; set; }
    }
}
