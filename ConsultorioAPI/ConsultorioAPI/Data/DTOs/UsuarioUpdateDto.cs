namespace ConsultorioAPI.Data.DTOs
{
    public class UsuarioUpdateDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }  
        public int RolId { get; set; }
    }
}
