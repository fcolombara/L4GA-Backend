using L4GA.Backend.Models;

namespace L4GA.Backend.Interfaces
{
    public interface IUsuarioRepository
    {
        // Para listar todos los operarios/admins
        Task<IEnumerable<Usuario>> GetAllAsync();

        // El "?" indica que puede devolver null si el ID no existe
        Task<Usuario?> GetByIdAsync(int id);
        Task DeleteAsync(int id);

        // ¡Este es el que faltaba para guardar en MySQL!
        Task<Usuario> CreateAsync(Usuario usuario);

        // Para actualizar datos o cambiar el Rol
        Task UpdateAsync(Usuario usuario);

        // Opcional: Para el futuro Login (buscar por email o nombre de usuario)
        Task<Usuario?> GetByEmailAsync(string email);
    }
}