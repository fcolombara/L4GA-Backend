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
    }
}
