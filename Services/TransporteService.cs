using L4GA.Backend.Interfaces;
using L4GA.Interfaces;
using L4GA.Models;

namespace L4GA.Backend.Services
{
    public class TransporteService : ITransporteService
    {
        private readonly ITransporteRepository _repo;

        public TransporteService(ITransporteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Transporte>> ListarTransportesAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Transporte?> ObtenerPorIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Transporte> RegistrarTransporteAsync(Transporte transporte)
        {
            // Aquí podrías agregar validaciones de negocio antes de guardar
            return await _repo.AddAsync(transporte);
        }

        public async Task<bool> EliminarTransporteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
