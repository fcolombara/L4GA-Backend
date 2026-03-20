using System;
using System.Collections.Generic;

namespace L4GA.Models
{
    public class NominaCreateDto
    {
        public DateTime FechaActividad { get; set; }
        public List<int> TransporteIds { get; set; } = new List<int>();

        public string? LinkTracking { get; set; }
    }
}
