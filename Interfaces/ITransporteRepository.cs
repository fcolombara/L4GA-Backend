using L4GA.Models;

namespace L4GA.Interfaces
{
    public interface ITransporteRepository
    {
        Task<IEnumerable<Transporte>> GetAllAsync();
        Task<Transporte?> GetByIdAsync(int id);
        Task<Transporte> AddAsync(Transporte transporte);
        Task<bool> DeleteAsync(int id);
    }
}
