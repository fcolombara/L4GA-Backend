using L4GA.Models;

namespace L4GA.Backend.Interfaces
{
    public interface ITransporteService
    {
        Task<IEnumerable<Transporte>> ListarTransportesAsync();
        Task<Transporte?> ObtenerPorIdAsync(int id);
        Task<Transporte> RegistrarTransporteAsync(Transporte transporte);
        Task<bool> EliminarTransporteAsync(int id);

        Task<bool> ActualizarTransporteAsync(Transporte transporte);
    }
}
