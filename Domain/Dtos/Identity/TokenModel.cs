using System.Text.Json.Serialization;

namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record TokenModel
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
