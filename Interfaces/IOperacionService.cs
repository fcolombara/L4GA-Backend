using L4GA.Backend.Models;
using L4GA.Backend.Repositories;

namespace L4GA.Backend.Interfaces
{
    public interface IOperacionService
    {
        Task<IEnumerable<Operacion>> ListarTodasAsync();
        Task<Operacion?> ObtenerPorIdAsync(int id);
        Task<Operacion> RegistrarSalidaCremerAsync(Operacion operacion);
        Task ActualizarOperacionAsync(Operacion operacion);
        Task EliminarOperacionAsync(int id);

        // Un método extra útil para filtrar operaciones de un camión específico
        Task<IEnumerable<Operacion>> ListarPorTransporteAsync(int transporteId);

        Task<IEnumerable<Operacion>> ListarPendientesGreenAsync();

        Task<IEnumerable<Operacion>> ObtenerPendientesSalidaGreenAsync();
        Task<IEnumerable<Operacion>> GetPendientesPuertoAsync();
    }
}