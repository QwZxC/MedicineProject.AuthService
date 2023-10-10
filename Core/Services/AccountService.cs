using MedicineProject.AuthService.Domain.Dtos.Identity;
using MedicineProject.AuthService.Domain.Extensions;
using MedicineProject.AuthService.Domain.Models;
using MedicineProject.AuthService.Domain.Repositories;
using MedicineProject.AuthService.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicineProject.AuthServices.Core.Service
{
    /// <summary>
    /// Сервис для работы с пользовательскими аккаунтами
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly UserManager<Patient> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountRepositroy _repository;

        public AccountService(UserManager<Patient> userManager, IAccountRepositroy repository, 
                              IConfiguration configuration) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<bool> CheckPasswordAsync(Patient user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public Patient CreateUser(RegisterRequest request)
        {
            return new Patient
            {
                Name = request.FirstName,
                Surname = request.LastName,
                Patronymic = request.MiddleName,
                Email = request.Email,
                UserName = request.Email,
            };
        }

        public async Task<Patient?> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<Patient?> FindByEmailInDbAsync(string email)
        {
            return await _repository.FindUserByEmailAsync(email);
        }

        public async Task<List<long>> FindRoleIdsAsync(long userId)
        {
            return await _repository.FindRoleIdsAsync(userId);
        }

        public async Task<List<IdentityRole<long>>> FindRolesAsync(List<long> roleIds)
        {
            return await _repository.FindRolesAsync(roleIds);
        }

        public string GenerateRefreshToken()
        {
            return _configuration.GenerateRefreshToken();
        }

        public DateTime GetRefreshTokenExpiryTime()
        {
            return DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());
        }

        public async Task<IdentityResult> GetResultAsync(Patient user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityRole<long>> GetRoleByNameAsync(string name)
        {
            return await _repository.FindRoleByNameAsync(name);
        }

        public async Task SaveChangedAsync()
        {
            await _repository.SaveChangesAsync();
        }

        public async Task LinkUserRole(Patient user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<Patient> FindUserByNameAsync(string name)
        {
           return await _userManager.FindByNameAsync(name);
        }

        public async Task UpdateUserAsync(Patient user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async Task RevokeAllAsync()
        {
            List<Patient> users = _userManager.Users.ToList();
            foreach (Patient user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
        }

        public ClaimsPrincipal GetPrincipal(string accessToken)
        {
            return _configuration.GetPrincipalFromExpiredToken(accessToken);
        }

        public JwtSecurityToken CreateToken(ClaimsPrincipal principal)
        {
            return _configuration.CreateToken(principal.Claims.ToList());
        }

        public void TrimProperties<TRequest>(TRequest request)
        {
            var type = typeof(TRequest);

            var properties = type.GetProperties();

            foreach(var prop in properties) 
            {
                var value = prop.GetValue(request);

                if (value is string str)
                {
                    prop.SetValue(request, str.Trim());
                }
            }
        }
    }
}
