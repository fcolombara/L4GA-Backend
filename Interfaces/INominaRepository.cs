using L4GA.Models;

namespace L4GA.Interfaces
{
    public interface INominaRepository
    {
        // 1. Modificamos o agregamos el método filtrado
        Task<IEnumerable<Nomina>> GetFilteredAsync(
            DateTime? inicioCarga,
            DateTime? finCarga,
            DateTime? inicioActividad,
            DateTime? finActividad);

        Task<IEnumerable<Nomina>> GetAllAsync();

        Task<Nomina> AddAsync(Nomina nomina, List<int> transporteIds);

        Task<bool> UpdateTrackingAsync(int id, string link);
    }
}