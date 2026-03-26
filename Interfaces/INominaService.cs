using L4GA.Models;

namespace L4GA.Backend.Interfaces
{
    public interface INominaService
    {
        Task<IEnumerable<Nomina>> ListarNominasAsync();
        Task<Nomina> CrearNominaAsync(NominaCreateDto dto);

        // Agregamos este nuevo método:
        Task<bool> ActualizarTrackingAsync(int id, string link);
        Task<IEnumerable<Nomina>> ListarNominasAsync(DateTime? inicioCarga, DateTime? finCarga, DateTime? inicioActividad, DateTime? finActividad);
    }
}
