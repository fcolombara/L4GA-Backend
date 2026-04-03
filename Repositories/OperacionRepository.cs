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
            // Usamos Include para traer los datos del Transporte (Patente, Chofer, etc.)
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
        public async Task<IEnumerable<Operacion>> GetPendientesSalidaGreenAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                // FILTRO CLAVE: Entró pero no salió
                .Where(o => o.LitrosInGreen > 0 && (o.LitrosOutGreen == 0 || o.LitrosOutGreen == null))
                .ToListAsync();
        }
        public async Task<IEnumerable<Operacion>> GetPendientesPuertoAsync()
        {
            return await _context.Operaciones
                .Include(o => o.Transporte)
                // Condición: Ya salió de Green (LitrosOut > 0) pero no entró al puerto (HoraIn == null)
                .Where(o => o.LitrosOutGreen > 0 && o.HoraInPuerto == null)
                .ToListAsync();
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
    }
}