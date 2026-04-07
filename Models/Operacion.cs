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
        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }

        [Required]
        public int TransporteId { get; set; }

        [ForeignKey("TransporteId")]
        public virtual Transporte? Transporte { get; set; }

        // --- ETAPA 1: SALIDA CREMER ---
        public TimeSpan? HoraArriboCremer { get; set; }
        public TimeSpan? HoraCargaNeutroCremer { get; set; }
        public TimeSpan? HoraOutCremer { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? TaraCremer { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesoCargadoCremer { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesoTotalCremer { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? LitrosCremer { get; set; } // Cálculo: PesoCargado * 0.92

        // --- ETAPA 2: INGRESO GREEN OIL ---
        public TimeSpan? HoraArriboGreen { get; set; }
        public TimeSpan? HoraInPlantaGreen { get; set; }
        public TimeSpan? HoraDescargaGreen { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? VolDescargadoGreen { get; set; } // Litros

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesoGreenIngreso { get; set; } // Cálculo: VolDescargado * 1.08695

        // --- ETAPA 3: EGRESO GREEN OIL ---
        public TimeSpan? HoraEquipoListoGreen { get; set; }
        public TimeSpan? HoraCargaBioGreen { get; set; }
        public TimeSpan? HoraOutGreen { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? VolCargadoGreen { get; set; } // Litros

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesoGreenEgreso { get; set; } // Cálculo: VolCargado * 1.13636

        // --- ETAPA 4: INGRESO A PUERTO ---
        public TimeSpan? HoraArriboPuerto { get; set; }
        public TimeSpan? HoraInUnidadPuerto { get; set; }

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesajePuerto { get; set; } // kg

        [Column(TypeName = "decimal(18,3)")]
        public decimal? PesoRecibidoPuerto { get; set; } // kg

        [Column(TypeName = "decimal(18,3)")]
        public decimal? LitrosRecibidosPuerto { get; set; } // Cálculo: PesoRecibido * 0.88

        public string? TrackingLink { get; set; }
    }
}