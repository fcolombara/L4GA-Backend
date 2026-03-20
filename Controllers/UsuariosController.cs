using Microsoft.AspNetCore.Mvc;
using L4GA.Backend.Models;
using L4GA.Backend.Interfaces;

namespace L4GA.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.ObtenerPorId(id);
            if (usuario == null) return NotFound(new { mensaje = $"ID {id} no encontrado." });
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            var nuevoUsuario = await _usuarioService.CrearUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = nuevoUsuario.Id }, nuevoUsuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _usuarioService.Login(request.Email, request.Password);

                if (usuario == null)
                {
                    return Unauthorized(new { mensaje = "Credenciales incorrectas." });
                }

                return Ok(new
                {
                    id = usuario.Id,
                    nombre = usuario.Nombre,
                    rol = usuario.Rol,
                    email = usuario.Email
                });
            }
            catch (Exception ex)
            {
                return StatusCode(403, new { mensaje = ex.Message });
            }
        }

        // --- CAMBIO CLAVE: Ruta más explícita para evitar el 404 ---
        [HttpPut("actualizar-rol/{id}")]
        public async Task<IActionResult> UpdateRol(int id, [FromBody] RolUpdateDto request)
        {
            try
            {
                // Agregamos un log para confirmar que el dato llega al entrar al método
                Console.WriteLine($"Petición recibida para ID: {id}, Nuevo Rol: {request?.NuevoRol}");

                var resultado = await _usuarioService.CambiarRol(id, request.NuevoRol);

                if (!resultado)
                {
                    return BadRequest(new { mensaje = "No se pudo actualizar el rol. Verifique el ID." });
                }

                return Ok(new { mensaje = "Rol actualizado con éxito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var eliminado = await _usuarioService.EliminarUsuario(id);
            if (!eliminado) return NotFound();
            return NoContent();
        }

        // --- DTOS ---
        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class RolUpdateDto
        {
            public string NuevoRol { get; set; } = string.Empty;
        }
    }
}