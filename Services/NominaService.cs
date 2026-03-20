using L4GA.Backend.Interfaces;
using L4GA.Interfaces;
using L4GA.Models;

namespace L4GA.Backend.Services
{
    public class NominaService : INominaService
    {
        private readonly INominaRepository _repo;

        public NominaService(INominaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Nomina>> ListarNominasAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Nomina> CrearNominaAsync(NominaCreateDto dto)
        {
            var nuevaNomina = new Nomina
            {
                FechaActividad = dto.FechaActividad,
                FechaCarga = DateTime.Now,
                // Si el DTO ya trae el link al crear, lo asignamos:
                LinkTracking = dto.LinkTracking
            };

            return await _repo.AddAsync(nuevaNomina, dto.TransporteIds);
        }

        // --- NUEVO MÉTODO IMPLEMENTADO ---
        public async Task<bool> ActualizarTrackingAsync(int id, string link)
        {
            // Le delegamos la actualización al repositorio
            return await _repo.UpdateTrackingAsync(id, link);
        }
    }
}