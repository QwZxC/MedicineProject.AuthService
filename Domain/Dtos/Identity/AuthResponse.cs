using System.Text.Json.Serialization;

namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record AuthResponse
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = null!;
        
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;

        [JsonPropertyName("refresh-token")]
        public string RefreshToken { get; set; } = null!;
        
    }
}
