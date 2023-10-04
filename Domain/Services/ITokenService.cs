using MedicineProject.AuthService.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace MedicineProject.AuthService.Domain.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Создаёт access-токен.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        string CreateToken(Patient user, List<IdentityRole<long>> role);
    }
}
