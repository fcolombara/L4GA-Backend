using System.Text.Json.Serialization;

namespace L4GA.Models
{
    public class Transporte
    {
        public int Id { get; set; }
        public string Chofer { get; set; } = string.Empty;
        public string Tracto { get; set; } = string.Empty; // Patente/Matrícula del camión
        public string Cisterna { get; set; } = string.Empty; // Patente/Matrícula del acoplado
        public string AnioTracto { get; set; } = string.Empty;
        public string AnioCisterna { get; set; } = string.Empty;

        public string? Contacto { get; set; } 

        [JsonIgnore]
        public List<Nomina> Nominas { get; set; } = new List<Nomina>();
    }
}