using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L4GA.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionesController : ControllerBase
    {
        private readonly IOperacionService _operacionService;

        public OperacionesController(IOperacionService operacionService)
        {
            _operacionService = operacionService;
        }

        // GET: api/Operaciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetOperaciones()
        {
            var operaciones = await _operacionService.ListarTodasAsync();
            return Ok(operaciones);
        }

        // GET: api/Operaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Operacion>> GetOperacion(int id)
        {
            var operacion = await _operacionService.ObtenerPorIdAsync(id);
            if (operacion == null) return NotFound();
            return Ok(operacion);
        }

        // POST: api/Operaciones (SALIDA CREMER)
        [HttpPost]
        public async Task<ActionResult<Operacion>> PostOperacion(Operacion operacion)
        {
            var nuevaOperacion = await _operacionService.RegistrarSalidaCremerAsync(operacion);
            return CreatedAtAction(nameof(GetOperacion), new { id = nuevaOperacion.Id }, nuevaOperacion);
        }

        // PUT: api/Operaciones/5 (ACTUALIZACIÓN GENÉRICA)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperacion(int id, Operacion operacion)
        {
            if (id != operacion.Id) return BadRequest();
            await _operacionService.ActualizarOperacionAsync(operacion);
            return NoContent();
        }

        // Probá cambiarlo a esto solo para testear:
        [HttpPatch("ingreso-green/{id}")]
        public async Task<IActionResult> PatchIngresoGreen(int id, [FromBody] OperacionIngresoDTO datos)
        {
            var operacion = await _operacionService.ObtenerPorIdAsync(id);
            if (operacion == null) return NotFound();

            // Intentamos parsear la hora
            if (TimeSpan.TryParse(datos.HoraInGreen, out TimeSpan horaConvertida))
            {
                operacion.HoraInGreen = horaConvertida;
                operacion.LitrosInGreen = datos.LitrosInGreen;

                await _operacionService.ActualizarOperacionAsync(operacion);
                return NoContent(); // RUTA 1: ÉXITO
            }

            // SI LLEGA ACÁ ES PORQUE EL TRYPARSE FALLÓ
            return BadRequest("El formato de hora debe ser HH:mm"); // RUTA 2: ERROR DE FORMATO
        }
        [HttpGet("pendientes-salida-green")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesSalida()
        {
            // Llamamos al service que vamos a crear ahora
            var operaciones = await _operacionService.ObtenerPendientesSalidaGreenAsync();
            return Ok(operaciones);
        }

        [HttpPatch("salida-green/{id}")]
        public async Task<IActionResult> PatchSalidaGreen(int id, [FromBody] OperacionSalidaDTO datos)
        {
            var operacion = await _operacionService.ObtenerPorIdAsync(id);
            if (operacion == null) return NotFound();

            if (TimeSpan.TryParse(datos.HoraOutGreen, out TimeSpan hora))
            {
                operacion.HoraOutGreen = hora;
                operacion.LitrosOutGreen = datos.LitrosOutGreen;
                await _operacionService.ActualizarOperacionAsync(operacion);
                return NoContent();
            }
            return BadRequest("Formato de hora inválido (HH:mm)");
        }



        // DELETE: api/Operaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperacion(int id)
        {
            await _operacionService.EliminarOperacionAsync(id);
            return NoContent();
        }

        // GET: api/Operaciones/Transporte/5
        [HttpGet("Transporte/{transporteId}")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPorTransporte(int transporteId)
        {
            var operaciones = await _operacionService.ListarPorTransporteAsync(transporteId);
            return Ok(operaciones);
        }
        [HttpGet("pendientes-puerto")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesPuerto()
        {
            // Usamos el Service, NO el _context
            var pendientes = await _operacionService.GetPendientesPuertoAsync();

            if (pendientes == null) return NotFound();

            return Ok(pendientes);
        }
        [HttpPatch("ingreso-puerto/{id}")]
        public async Task<IActionResult> ActualizarIngresoPuerto(int id, [FromBody] Operacion puertoDatos)
        {
            // 1. Buscamos la operación existente
            var operacion = await _operacionService.ObtenerPorIdAsync(id);

            if (operacion == null) return NotFound("No se encontró la operación.");

            // 2. Mapeamos solo los campos que vienen del formulario de Puerto
            operacion.HoraInPuerto = puertoDatos.HoraInPuerto;
            operacion.PesoInPuerto = puertoDatos.PesoInPuerto;
            operacion.LitrosInPuerto = puertoDatos.LitrosInPuerto;

            // 3. Guardamos los cambios a través del Service
            await _operacionService.ActualizarOperacionAsync(operacion);

            return NoContent();
        }

        // GET: api/Operaciones/pendientes-green
        [HttpGet("pendientes-green")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesGreen()
        {
            var pendientes = await _operacionService.ListarPendientesGreenAsync();
            return Ok(pendientes);
        }
        
    }


    // DTO para evitar enviar todo el objeto Operacion desde el Frontend de Green Oil
    public class OperacionIngresoDTO
    {
        public string HoraInGreen { get; set; } // Lo recibimos como string para parsearlo a mano
        public int LitrosInGreen { get; set; }
    }
    public class OperacionSalidaDTO
    {
        public string HoraOutGreen { get; set; }
        public int LitrosOutGreen { get; set; }
    }
    public class OperacionPuertoDTO
    {
        public string HoraInPuerto { get; set; }
        public decimal? PesoInPuerto { get; set; }
        public int? LitrosInPuerto { get; set; }
    }

}