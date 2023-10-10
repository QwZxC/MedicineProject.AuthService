namespace MedicineProject.AuthService.Domain.Dtos.Identity
{
    public record AuthResponse
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
