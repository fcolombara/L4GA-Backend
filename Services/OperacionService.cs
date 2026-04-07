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

        public OperacionService(IOperacionRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<IEnumerable<Operacion>> ListarTodasAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte) // <--- ESTO ES VITAL
                .OrderByDescending(o => o.Fecha) // Opcional: ver las últimas primero
                .ToListAsync();
        }
        public async Task<Operacion?> ObtenerPorIdAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<Operacion>> ListarPorTransporteAsync(int transporteId) => await _repository.GetByTransporteIdAsync(transporteId);

        public async Task<Operacion> RegistrarSalidaCremerAsync(Operacion operacion)
        {
            if (operacion.Fecha == default) operacion.Fecha = DateTime.Now.Date;

            // --- LÓGICA ETAPA 1: SALIDA CREMER ---
            // Litros = 92% del Peso Cargado
            if (operacion.PesoCargadoCremer.HasValue)
            {
                operacion.LitrosCremer = operacion.PesoCargadoCremer.Value * 0.92m;
            }

            return await _repository.CreateAsync(operacion);
        }

        public async Task ActualizarOperacionAsync(Operacion operacion)
        {
            // --- LÓGICA ETAPA 2: INGRESO GREEN ---
            // Peso Ingreso = 108,695% del Volumen Descargado
            if (operacion.VolDescargadoGreen.HasValue)
            {
                operacion.PesoGreenIngreso = operacion.VolDescargadoGreen.Value * 1.08695m;
            }

            // --- LÓGICA ETAPA 3: EGRESO GREEN ---
            // Peso Egreso = 113,636% del Volumen Cargado
            if (operacion.VolCargadoGreen.HasValue)
            {
                operacion.PesoGreenEgreso = operacion.VolCargadoGreen.Value * 1.13636m;
            }

            // --- LÓGICA ETAPA 4: INGRESO A PUERTO ---
            // Litros Recibidos = 88% del Peso Recibido
            if (operacion.PesoRecibidoPuerto.HasValue)
            {
                operacion.LitrosRecibidosPuerto = operacion.PesoRecibidoPuerto.Value * 0.88m;
            }

            await _repository.UpdateAsync(operacion);
        }
        public async Task ActualizarTracking(int id, string link)
        {
            var operacion = await _context.Operaciones.FindAsync(id);
            if (operacion != null)
            {
                operacion.TrackingLink = link;
                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarOperacionAsync(int id) => await _repository.DeleteAsync(id);

        public async Task<IEnumerable<Operacion>> ObtenerPendientesSalidaGreenAsync() => await _repository.GetPendientesSalidaGreenAsync();

        public async Task<IEnumerable<Operacion>> GetPendientesPuertoAsync() => await _repository.GetPendientesPuertoAsync();

        public async Task<IEnumerable<Operacion>> ListarPendientesGreenAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .Where(o => o.HoraOutCremer != null && o.HoraArriboGreen == null)
                .ToListAsync();
        }
    }
}