using MedicineProject.AuthService.Domain.Models;

using Microsoft.AspNetCore.Identity;

namespace MedicineProject.AuthService.Domain.Repositories
{
    public interface IAccountRepositroy : IBaseRepository
    {
        /// <summary>
        /// Осуществляет поиск пользователя по email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<Patient> FindUserByEmailAsync(string email);

        /// <summary>
        /// Осуществляет поиск id ролей пользователя.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> FindRoleIdsAsync(long userId);
        
        /// <summary>
        /// Осуществляет поиск списка ролей по их id.
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<IdentityRole<long>>> FindRolesAsync(List<long> roleIds);
        
        /// <summary>
        /// Осуществляет поиск роли, по её названию.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IdentityRole<long>> FindRoleByNameAsync(string name);
    }
}
