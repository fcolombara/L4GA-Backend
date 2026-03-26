using L4GA.Backend.Interfaces;
using L4GA.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4GA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransporteController : ControllerBase
    {
        private readonly ITransporteService _transporteService;

        public TransporteController(ITransporteService transporteService)
        {
            _transporteService = transporteService;
        }

        // GET: api/Transporte
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transporte>>> GetTransportes()
        {
            var transportes = await _transporteService.ListarTransportesAsync();
            return Ok(transportes);
        }

        // GET: api/Transporte/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transporte>> GetTransporte(int id)
        {
            var transporte = await _transporteService.ObtenerPorIdAsync(id);

            if (transporte == null)
            {
                return NotFound(new { message = "Transporte no encontrado" });
            }

            return Ok(transporte);
        }
        // PUT: api/Transporte/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransporte(int id, Transporte transporte)
        {
            if (id != transporte.Id)
            {
                return BadRequest(new { message = "El ID no coincide con el transporte enviado" });
            }

            // Llamamos al servicio para actualizar (incluyendo el nuevo campo Contacto)
            var actualizado = await _transporteService.ActualizarTransporteAsync(transporte);

            if (!actualizado)
            {
                return NotFound(new { message = "No se pudo actualizar: el transporte no existe" });
            }

            return NoContent(); // 204: Éxito pero no devuelve contenido (estándar para PUT)
        }

        // POST: api/Transporte
        [HttpPost]
        public async Task<ActionResult<Transporte>> PostTransporte(Transporte transporte)
        {
            var nuevoTransporte = await _transporteService.RegistrarTransporteAsync(transporte);

            return CreatedAtAction(nameof(GetTransporte), new { id = nuevoTransporte.Id }, nuevoTransporte);
        }

        // DELETE: api/Transporte/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransporte(int id)
        {
            var eliminado = await _transporteService.EliminarTransporteAsync(id);

            if (!eliminado)
            {
                return NotFound(new { message = "No se pudo eliminar: el transporte no existe" });
            }

            return NoContent();
        }
    }


}