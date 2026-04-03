using L4GA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L4GA.Backend.Models
{
    public class Operacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")] // <--- ESTO ES CLAVE para que coincida con MySQL
        public DateTime Fecha { get; set; }

        [Required]
        public int TransporteId { get; set; }

        // Propiedad de navegación para traer los datos del camión/chofer
        [ForeignKey("TransporteId")]
        public virtual Transporte? Transporte { get; set; }

        // --- Datos de CREMER ---
        public TimeSpan? HoraOutCremer { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PesoCremer { get; set; }

        public int? LitrosCremer { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? TaraCremer { get; set; }

        // --- Datos de GREEN (Entrada/Salida) ---
        public TimeSpan? HoraInGreen { get; set; }
        public int? LitrosInGreen { get; set; }

        public TimeSpan? HoraOutGreen { get; set; }
        public int? LitrosOutGreen { get; set; }

        // --- Datos de PUERTO ---
        public TimeSpan? HoraInPuerto { get; set; }

        public decimal? PesoInPuerto { get; set; }
        public int? LitrosInPuerto { get; set; }

        public string? TrackingLink { get; set; }
    }
}