using BCrypt.Net;
using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;

namespace L4GA.Backend.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;
        public UsuarioService(IUsuarioRepository repo) => _repo = repo;

        public async Task<IEnumerable<Usuario>> ListarUsuarios() => await _repo.GetAllAsync();

        public async Task<Usuario?> ObtenerPorId(int id) => await _repo.GetByIdAsync(id);

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            // --- MEJORA: Aseguramos que el Rol nunca sea nulo al guardar ---
            if (string.IsNullOrWhiteSpace(usuario.Rol))
            {
                usuario.Rol = "Consulta";
            }

            usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash);
            return await _repo.CreateAsync(usuario);
        }

        // --- Login con validación de estado ---
        public async Task<Usuario?> Login(string email, string passwordPlana)
        {
            var usuario = await _repo.GetByEmailAsync(email);

            if (usuario == null) return null;

            // 1. Verificamos la contraseña
            bool passwordValida = BCrypt.Net.BCrypt.Verify(passwordPlana, usuario.PasswordHash);
            if (!passwordValida) return null;

            // 2. Chequeamos que no esté de baja
            if (usuario.Rol == "Baja")
            {
                throw new Exception("Tu cuenta ha sido inhabilitada. Contacta al administrador.");
            }

            return usuario;
        }

        // --- Gestión de roles permitidos ---
        public async Task<bool> CambiarRol(int id, string nuevoRol)
        {
            var usuario = await _repo.GetByIdAsync(id);
            if (usuario == null) return false;

            // 1. Limpiamos cualquier espacio o salto de línea que venga del front
            var rolRecibido = nuevoRol?.Trim();

            // 2. Usamos una lista clara y comparamos ignorando mayúsculas/minúsculas
            var rolesValidos = new List<string> { "Admin", "Operador", "Operario", "Consulta", "Baja" };

            bool esValido = rolesValidos.Any(r => string.Equals(r, rolRecibido, StringComparison.OrdinalIgnoreCase));

            if (!esValido)
            {
                // Esto saldrá en la consola de Visual Studio para que veas qué está llegando exactamente
                Console.WriteLine($" ERROR: El rol '{rolRecibido}' no coincide con nada en la lista permitida.");
                return false;
            }

            // 3. Si es válido, asignamos el valor de la lista para asegurar consistencia
            usuario.Rol = rolesValidos.First(r => string.Equals(r, rolRecibido, StringComparison.OrdinalIgnoreCase));

            await _repo.UpdateAsync(usuario);
            return true;
        }
        public async Task<bool> EliminarUsuario(int id)
        {
            var usuario = await _repo.GetByIdAsync(id);
            if (usuario == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }
    }
    
    }