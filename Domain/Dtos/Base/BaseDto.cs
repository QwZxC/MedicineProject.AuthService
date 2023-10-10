using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicineProject.AuthService.Domain.Dtos.Base
{
    public record BaseDto
    {
        [Key]
        [JsonPropertyName("name")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
