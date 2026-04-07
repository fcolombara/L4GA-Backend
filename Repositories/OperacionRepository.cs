using L4GA.Backend.Data;
using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Backend.Repositories
{
    public class OperacionRepository : IOperacionRepository
    {
        private readonly ApplicationDbContext _context;

        public OperacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Operacion>> GetAllAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();
        }

        public async Task<Operacion?> GetByIdAsync(int id)
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Operacion>> GetByTransporteIdAsync(int transporteId)
        {
            return await _context.Operaciones
                .Where(o => o.TransporteId == transporteId)
                .ToListAsync();
        }

        public async Task<Operacion> CreateAsync(Operacion operacion)
        {
            await _context.Operaciones.AddAsync(operacion);
            await _context.SaveChangesAsync();
            return operacion;
        }

        public async Task UpdateAsync(Operacion operacion)
        {
            _context.Entry(operacion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var operacion = await _context.Operaciones.FindAsync(id);
            if (operacion != null)
            {
                _context.Operaciones.Remove(operacion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Operacion>> GetPendientesSalidaGreenAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .Where(o => o.VolDescargadoGreen > 0 && (o.VolCargadoGreen == 0 || o.VolCargadoGreen == null))
                .ToListAsync();
        }

        public async Task<IEnumerable<Operacion>> GetPendientesPuertoAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                .Where(o => o.VolCargadoGreen > 0 && o.HoraArriboPuerto == null)
                .ToListAsync();
        }
    }
}