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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetOperaciones()
        {
            var operaciones = await _operacionService.ListarTodasAsync();
            return Ok(operaciones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Operacion>> GetOperacion(int id)
        {
            var operacion = await _operacionService.ObtenerPorIdAsync(id);
            if (operacion == null) return NotFound();
            return Ok(operacion);
        }

        [HttpPost]
        public async Task<ActionResult<Operacion>> PostOperacion(Operacion operacion)
        {
            var nuevaOperacion = await _operacionService.RegistrarSalidaCremerAsync(operacion);
            return CreatedAtAction(nameof(GetOperacion), new { id = nuevaOperacion.Id }, nuevaOperacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutOperacion(int id, Operacion operacion)
        {
            if (id != operacion.Id) return BadRequest();
            await _operacionService.ActualizarOperacionAsync(operacion);
            return NoContent();
        }

        [HttpPatch("ingreso-green/{id}")]
        public async Task<IActionResult> PatchIngresoGreen(int id, [FromBody] Operacion datos)
        {
            // 1. Buscamos la operación real de la DB
            var operacionDb = await _operacionService.ObtenerPorIdAsync(id);
            if (operacionDb == null) return NotFound();

            // 2. Solo actualizamos los campos de la Etapa 2
            // No hace falta TryParse porque 'datos' ya trae objetos TimeSpan
            operacionDb.HoraArriboGreen = datos.HoraArriboGreen;
            operacionDb.HoraInPlantaGreen = datos.HoraInPlantaGreen;
            operacionDb.HoraDescargaGreen = datos.HoraDescargaGreen;
            operacionDb.VolDescargadoGreen = datos.VolDescargadoGreen;

            // 3. Guardamos
            await _operacionService.ActualizarOperacionAsync(operacionDb);
            return NoContent();
        }

        [HttpGet("pendientes-salida-green")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesSalida()
        {
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
                operacion.VolCargadoGreen = datos.VolCargadoGreen;
                await _operacionService.ActualizarOperacionAsync(operacion);
                return NoContent();
            }
            return BadRequest("Formato de hora inválido (HH:mm)");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOperacion(int id)
        {
            await _operacionService.EliminarOperacionAsync(id);
            return NoContent();
        }

        [HttpGet("Transporte/{transporteId}")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPorTransporte(int transporteId)
        {
            var operaciones = await _operacionService.ListarPorTransporteAsync(transporteId);
            return Ok(operaciones);
        }

        [HttpGet("pendientes-puerto")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesPuerto()
        {
            var pendientes = await _operacionService.GetPendientesPuertoAsync();
            if (pendientes == null) return NotFound();
            return Ok(pendientes);
        }

        [HttpPatch("ingreso-puerto/{id}")]
        public async Task<IActionResult> ActualizarIngresoPuerto(int id, [FromBody] OperacionPuertoDTO puertoDatos)
        {
            var operacion = await _operacionService.ObtenerPorIdAsync(id);
            if (operacion == null) return NotFound("No se encontró la operación.");

            if (TimeSpan.TryParse(puertoDatos.HoraInPuerto, out TimeSpan hora))
            {
                operacion.HoraArriboPuerto = hora;
                operacion.PesajePuerto = puertoDatos.PesajePuerto;
                operacion.PesoRecibidoPuerto = puertoDatos.PesoRecibidoPuerto;

                await _operacionService.ActualizarOperacionAsync(operacion);
                return NoContent();
            }
            return BadRequest("Formato de hora inválido");
        }
        public class TrackingDTO
        {
            public string TrackingLink { get; set; }
        }

        // En el controlador:
        [HttpPut("ActualizarTracking/{id}")]
        public async Task<IActionResult> UpdateTracking(int id, [FromBody] TrackingDTO data)
        {
            await _operacionService.ActualizarTracking(id, data.TrackingLink);
            return NoContent();
        }

        [HttpGet("pendientes-green")]
        public async Task<ActionResult<IEnumerable<Operacion>>> GetPendientesGreen()
        {
            var pendientes = await _operacionService.ListarPendientesGreenAsync();
            return Ok(pendientes);
        }
    }

    public class OperacionIngresoDTO
    {
        // Usá EXACTAMENTE los mismos nombres que en Operacion.cs
        public string? HoraArriboGreen { get; set; }
        public string? HoraInPlantaGreen { get; set; }
        public string? HoraDescargaGreen { get; set; }
        public decimal? VolDescargadoGreen { get; set; }
    }

    public class OperacionSalidaDTO
    {
        public string? HoraOutGreen { get; set; }
        public decimal? VolCargadoGreen { get; set; }
    }
    public class TrackingDTO
    {
        public string TrackingLink { get; set; }
    }

    public class OperacionPuertoDTO
    {
        public string? HoraInPuerto { get; set; }
        public decimal? PesajePuerto { get; set; }
        public decimal? PesoRecibidoPuerto { get; set; }
    }
}