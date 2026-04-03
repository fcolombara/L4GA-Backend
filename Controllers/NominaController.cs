using L4GA.Backend.Interfaces;
using L4GA.Backend.Models;
using L4GA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace L4GA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NominaController : ControllerBase
    {
       
        private readonly INominaService _nominaService;

        public NominaController(INominaService nominaService)
        {
            _nominaService = nominaService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nomina>>> GetNominas(
    [FromQuery] DateTime? inicioCarga,
    [FromQuery] DateTime? finCarga,
    [FromQuery] DateTime? inicioActividad,
    [FromQuery] DateTime? finActividad)
        {
            // Pasamos los 4 parámetros al servicio
            var nominas = await _nominaService.ListarNominasAsync(inicioCarga, finCarga, inicioActividad, finActividad);
            return Ok(nominas);
        }

        [HttpPost]
        public async Task<ActionResult<Nomina>> PostNomina(NominaCreateDto dto)
        {
            if (dto.TransporteIds == null || dto.TransporteIds.Count == 0)
            {
                return BadRequest("Debe seleccionar al menos un transporte.");
            }

            var nominaCreada = await _nominaService.CrearNominaAsync(dto);
            return Ok(nominaCreada);
        }

        [HttpPut("{id}/tracking")]
        public async Task<IActionResult> UpdateTracking(int id, [FromBody] string link)
        {
            // USAMOS EL SERVICE, NO EL CONTEXTO
            var resultado = await _nominaService.ActualizarTrackingAsync(id, link);

            if (!resultado)
            {
                return NotFound("No se encontró la nómina para actualizar el tracking.");
            }

            return NoContent();
        }
       
    }
}
