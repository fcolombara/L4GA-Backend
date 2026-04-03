using L4GA.Backend.Models;
// Asegurate de que este namespace coincida con donde creaste Operacion.cs
using L4GA.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Nomina> Nominas { get; set; }
        public DbSet<Transporte> Transportes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        // 1. REGISTRAMOS LA NUEVA TABLA
        public DbSet<Operacion> Operaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración Muchos a Muchos (Ya la tenías, queda igual)
            modelBuilder.Entity<Nomina>()
                .HasMany(n => n.Transportes)
                .WithMany(t => t.Nominas)
                .UsingEntity(j => j.ToTable("NominaTransportes"));

            // 2. CONFIGURACIÓN PARA OPERACIONES (Relación 1 a Muchos)
            modelBuilder.Entity<Operacion>()
                .HasOne(o => o.Transporte)
                .WithMany() // Un transporte puede tener muchas operaciones
                .HasForeignKey(o => o.TransporteId)
                .OnDelete(DeleteBehavior.Restrict); // Evita borrar transportes con viajes activos

            // 3. PRECISIÓN DE DECIMALES (Clave para balanza)
            modelBuilder.Entity<Operacion>()
                .Property(o => o.PesoCremer)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Operacion>()
                .Property(o => o.TaraCremer)
                .HasPrecision(10, 2);
        }
    }
}