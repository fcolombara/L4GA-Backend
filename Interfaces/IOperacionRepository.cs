using L4GA.Backend.Models;

namespace L4GA.Backend.Interfaces
{
    public interface IOperacionRepository
    {
        Task<IEnumerable<Operacion>> GetAllAsync();
        Task<Operacion?> GetByIdAsync(int id);
        Task<IEnumerable<Operacion>> GetByTransporteIdAsync(int transporteId);
        Task<Operacion> CreateAsync(Operacion operacion);
        Task UpdateAsync(Operacion operacion);
        Task DeleteAsync(int id);
        Task<IEnumerable<Operacion>> GetPendientesSalidaGreenAsync();
        Task<IEnumerable<Operacion>> GetPendientesPuertoAsync();

    }
}
