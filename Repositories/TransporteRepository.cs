using L4GA.Backend.Data;
using L4GA.Interfaces;
using L4GA.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Repositories
{
    public class TransporteRepository : ITransporteRepository
    {
        private readonly ApplicationDbContext _context;

        public TransporteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transporte>> GetAllAsync()
        {
            return await _context.Transportes.ToListAsync();
        }

        public async Task<Transporte?> GetByIdAsync(int id)
        {
            return await _context.Transportes.FindAsync(id);
        }

        public async Task<Transporte> AddAsync(Transporte transporte)
        {
            _context.Transportes.Add(transporte);
            await _context.SaveChangesAsync();
            return transporte;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transporte = await _context.Transportes.FindAsync(id);
            if (transporte == null) return false;

            _context.Transportes.Remove(transporte);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateAsync(Transporte transporte)
        {
            // Verificamos si la entidad existe en la base de datos
            var existe = await _context.Transportes.AnyAsync(t => t.Id == transporte.Id);
            if (!existe) return false;

            // Marcamos la entidad como modificada para que EF genere el UPDATE
            _context.Transportes.Update(transporte);

            try
            {
                // Guardamos cambios (esto actualizará Chofer, Tracto, Cisterna y el nuevo Contacto)
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
