namespace ConsultorioAPI.Data.DTOs
{
    public class ServicioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int DuracionMinutos { get; set; }
        public bool Activo { get; set; }
    }
}
