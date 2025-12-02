using ConsultorioAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Admin" },
                new Rol { Id = 2, Nombre = "Cliente" }
            );

            
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            
            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Pago)
                .WithOne(p => p.Turno)
                .HasForeignKey<Turno>(t => t.PagoId)
                .IsRequired(false);
        }
    }
}
