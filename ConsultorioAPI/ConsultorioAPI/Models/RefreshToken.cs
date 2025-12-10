using System;
using System.ComponentModel.DataAnnotations;

namespace ConsultorioAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime Expiration { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expiration;
    }
}
