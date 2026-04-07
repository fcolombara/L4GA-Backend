using System.ComponentModel.DataAnnotations;

namespace L4GA.Backend.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Aquí definimos el Rol (Admin, Operario, etc.)
        [Required]
        public string Rol { get; set; } = "Consulta";
    }
}
