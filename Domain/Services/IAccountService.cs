using MedicineProject.AuthService.Domain.DTOs.Identity;
using MedicineProject.AuthService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicineProject.AuthService.Domain.Services
{
    public interface IAccountService
    {
        /// <summary>
        /// Осуществляет поиск пользователя в userManager.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Patient?> FindUserByEmailAsync(string email);

        /// <summary>
        /// Провряет пароль на подлинность.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> CheckPasswordAsync(Patient user, string password);

        /// <summary>
        /// Осуществляет поиск существуещего пользователя по email в базе данных.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Patient?> FindByEmailInDbAsync(string email);

        /// <summary>
        /// Осуществляет поиск ролей по id пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> FindRoleIdsAsync(long userId);

        /// <summary>
        /// Осуществляет поиск ролей, по id.
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<IdentityRole<long>>> FindRolesAsync(List<long> roleIds);

        /// <summary>
        /// Генерирует Refresh-токен для пользователя.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Возвращает время истечения Refresh-токена.
        /// </summary>
        /// <returns></returns>
        DateTime GetRefreshTokenExpiryTime();

        /// <summary>
        /// Сохраняет изменения в бд
        /// </summary>
        /// <returns></returns>
        Task SaveChangedAsync();

        /// <summary>
        /// Получает роль по её названию.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IdentityRole<long>> GetRoleByNameAsync(string name);

        /// <summary>
        /// Возвращает модель пользователя.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Patient CreateUser(RegisterRequest request);

        /// <summary>
        /// Пытается создать пользователя и возвращает результат создания.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> GetResultAsync(Patient user, string password);

        /// <summary>
        /// Присваивает роль пользователю.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        Task LinkUserRole(Patient user, string role);

        /// <summary>
        /// Осуществляет поиск пользователя по userName.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Patient> FindUserByNameAsync(string name);

        /// <summary>
        /// Обновляет данные о пользователе.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(Patient user);

        /// <summary>
        /// Делает Refresh-токены всех пользователей null.
        /// </summary>
        /// <returns></returns>
        Task RevokeAllAsync();

        ClaimsPrincipal GetPrincipal(string accessToken);

        /// <summary>
        /// Создаёт Access-токен.
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        JwtSecurityToken CreateToken(ClaimsPrincipal principal);

        /// <summary>
        /// Убирает пробелы в строковых свойствах.
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        void TrimProperties<T>(T request);
    }
}
