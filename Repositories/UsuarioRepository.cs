using L4GA.Backend.Data;
using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Backend.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Usuario>> GetAllAsync() => await _context.Usuarios.ToListAsync();

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Usuario?> GetByIdAsync(int id) => await _context.Usuarios.FindAsync(id);

        // NUEVO: Método para guardar el usuario en MySQL
        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario; // Devolvemos el usuario ya con el ID generado por la DB
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            // Buscamos si la entidad ya está siendo rastreada para evitar conflictos de ID
            var local = _context.Usuarios.Local.FirstOrDefault(entry => entry.Id == usuario.Id);

            if (local != null)
            {
                // Si está en memoria, quitamos el rastreo de la versión vieja
                _context.Entry(local).State = EntityState.Detached;
            }

            // Marcamos el usuario que recibimos como Modificado
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Esto te dirá en la consola de Visual Studio si MySQL rechazó el cambio
                Console.WriteLine($"Error en SaveChanges: {ex.Message}");
                throw;
            }
        }

        // Recomendado: Para el futuro Login
        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}