using System.ComponentModel.DataAnnotations;

namespace L4GA.Models
{
    public class Nomina
    {
        [Key]
        public int Id { get; set; }

        // Se completa sola al crear el objeto
        public DateTime FechaCarga { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaActividad { get; set; }

        public string? LinkTracking { get; set; }

        // Relación: Una nómina tiene una lista de transportes
        public List<Transporte> Transportes { get; set; } = new List<Transporte>();
    }
}
