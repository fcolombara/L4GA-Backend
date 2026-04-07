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
        public DbSet<Operacion> Operaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración Muchos a Muchos (Queda igual)
            modelBuilder.Entity<Nomina>()
                .HasMany(n => n.Transportes)
                .WithMany(t => t.Nominas)
                .UsingEntity(j => j.ToTable("NominaTransportes"));

            // Configuración para Operaciones (Relación 1 a Muchos)
            modelBuilder.Entity<Operacion>()
                .HasOne(o => o.Transporte)
                .WithMany()
                .HasForeignKey(o => o.TransporteId)
                .OnDelete(DeleteBehavior.Restrict);

            // CONFIGURACIÓN DE PRECISIÓN (Cambiado a los nuevos nombres y 3 decimales)

            // Etapa 1
            modelBuilder.Entity<Operacion>().Property(o => o.TaraCremer).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.PesoCargadoCremer).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.PesoTotalCremer).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.LitrosCremer).HasPrecision(18, 3);

            // Etapa 2 y 3
            modelBuilder.Entity<Operacion>().Property(o => o.VolDescargadoGreen).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.PesoGreenIngreso).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.VolCargadoGreen).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.PesoGreenEgreso).HasPrecision(18, 3);

            // Etapa 4
            modelBuilder.Entity<Operacion>().Property(o => o.PesajePuerto).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.PesoRecibidoPuerto).HasPrecision(18, 3);
            modelBuilder.Entity<Operacion>().Property(o => o.LitrosRecibidosPuerto).HasPrecision(18, 3);
        }
    }
}