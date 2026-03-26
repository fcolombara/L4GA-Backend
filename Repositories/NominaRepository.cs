using L4GA.Backend.Data;
using L4GA.Interfaces;
using L4GA.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Repositories
{
    public class NominaRepository : INominaRepository
    {
        private readonly ApplicationDbContext _context;

        public NominaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nomina>> GetAllAsync()
        {
            return await _context.Nominas
                .Include(n => n.Transportes)
                .OrderByDescending(n => n.FechaCarga) // Opcional: para ver las últimas primero
                .ToListAsync();
        }

        public async Task<Nomina> AddAsync(Nomina nomina, List<int> transporteIds)
        {
            var transportesSeleccionados = await _context.Transportes
                .Where(t => transporteIds.Contains(t.Id))
                .ToListAsync();

            nomina.Transportes = transportesSeleccionados;

            _context.Nominas.Add(nomina);
            await _context.SaveChangesAsync();
            return nomina;
        }
        public async Task<IEnumerable<Nomina>> GetFilteredAsync(DateTime? inicioCarga, DateTime? finCarga, DateTime? inicioActividad, DateTime? finActividad)
        {
            // 1. Empezamos con la consulta base e incluimos los transportes
            var query = _context.Nominas.Include(n => n.Transportes).AsQueryable();

            // 2. Aplicamos filtros solo si vienen con datos
            if (inicioCarga.HasValue)
                query = query.Where(n => n.FechaCarga >= inicioCarga.Value);

            if (finCarga.HasValue)
                query = query.Where(n => n.FechaCarga <= finCarga.Value);

            if (inicioActividad.HasValue)
                query = query.Where(n => n.FechaActividad >= inicioActividad.Value);

            if (finActividad.HasValue)
                query = query.Where(n => n.FechaActividad <= finActividad.Value);

            // 3. Ejecutamos la consulta filtrada
            return await query.OrderByDescending(n => n.FechaCarga).ToListAsync();
        }

        // --- MÉTODO FINAL: IMPACTO EN BASE DE DATOS ---
        public async Task<bool> UpdateTrackingAsync(int id, string link)
        {
            var nomina = await _context.Nominas.FindAsync(id);

            if (nomina == null) return false;

            nomina.LinkTracking = link;

            // Solo guardamos los cambios en la fila de la nómina
            await _context.SaveChangesAsync();
            return true;
        }
    }
}