using L4GA.Backend.Models;

namespace L4GA.Backend.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> ListarUsuarios();
        Task<Usuario?> ObtenerPorId(int id);
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<bool> CambiarRol(int id, string nuevoRol);
        Task<bool> EliminarUsuario(int id);

        // NUEVO: Método para validar el acceso al sistema
        // Devolvemos el Usuario si es válido, o null si falla el email o la clave
        Task<Usuario?> Login(string email, string passwordPlana);
    }
}