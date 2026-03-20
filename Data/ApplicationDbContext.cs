using L4GA.Backend.Models;
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

        // Agregamos la configuración de relaciones
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuramos explícitamente la relación Muchos a Muchos
            modelBuilder.Entity<Nomina>()
                .HasMany(n => n.Transportes)
                .WithMany(t => t.Nominas)
                .UsingEntity(j => j.ToTable("NominaTransportes")); // Nombre de la tabla pivot
        }
    }
}