using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record AuthRequest
    {
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
