using L4GA.Backend.Data;
using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Backend.Services
{
    public class OperacionService : IOperacionService
    {
        private readonly IOperacionRepository _repository;
        private readonly ApplicationDbContext _context;

        // UN SOLO CONSTRUCTOR: Quitamos el de UnitOfWork que sobraba
        public OperacionService(IOperacionRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<Operacion>> ListarTodasAsync() => await _repository.GetAllAsync();

        public async Task<Operacion?> ObtenerPorIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<Operacion>> ListarPorTransporteAsync(int transporteId) => await _repository.GetByTransporteIdAsync(transporteId);

        public async Task<Operacion> RegistrarSalidaCremerAsync(Operacion operacion)
        {
            if (operacion.Fecha == default) operacion.Fecha = DateTime.Now.Date;
            return await _repository.CreateAsync(operacion);
        }

        public async Task ActualizarOperacionAsync(Operacion operacion) => await _repository.UpdateAsync(operacion);

        public async Task EliminarOperacionAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<IEnumerable<Operacion>> ObtenerPendientesSalidaGreenAsync() => await _repository.GetPendientesSalidaGreenAsync();

        // CORREGIDO: Usamos _repository como en los otros métodos
        public async Task<IEnumerable<Operacion>> GetPendientesPuertoAsync()
        {
            return await _repository.GetPendientesPuertoAsync();
        }

        public async Task<IEnumerable<Operacion>> ListarPendientesGreenAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .Where(o => o.HoraOutCremer != null && o.HoraInGreen == null)
                .ToListAsync();
        }
    }
}