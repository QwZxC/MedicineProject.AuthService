using System.ComponentModel.DataAnnotations;

namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record RegistrationRequest
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
