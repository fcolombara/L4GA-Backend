using L4GA.Models;

namespace L4GA.Interfaces
{
    public interface INominaRepository
    {
        Task<IEnumerable<Nomina>> GetAllAsync();
        Task<Nomina> AddAsync(Nomina nomina, List<int> transporteIds);

        // Agregamos el método para impactar en la Base de Datos
        Task<bool> UpdateTrackingAsync(int id, string link);
    }
}
