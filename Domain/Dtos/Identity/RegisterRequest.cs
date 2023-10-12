using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record RegisterRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Дата рождения")]
        [JsonPropertyName("birt_date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        [JsonPropertyName("password_confirm")]
        public string PasswordConfirm { get; set; } = null!;

        [Required]
        [Display(Name = "Имя")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Display(Name = "Фамилия")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;

        [Display(Name = "Отчество")]
        [JsonPropertyName("middle_name")]
        public string? MiddleName { get; set; }

        [Required]
        [JsonPropertyName("role")]
        public string Role { get; set; } = null!;
    }
}
