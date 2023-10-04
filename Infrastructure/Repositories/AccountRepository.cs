using MedicineProject.AuthService.Domain.Context;
using MedicineProject.AuthService.Domain.Models;
using MedicineProject.AuthService.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MedicineProject.AuthService.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepositroy
    {
        public AccountRepository(WebMobileContext context) : 
            base(context) 
        {
            
        }

        public async Task<IdentityRole<long>> FindRoleByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(role => name == role.Name);
        }

        public async Task<List<long>> FindRoleIdsAsync(long userId)
        {
            return await _context.UserRoles.Where(r => r.UserId == userId).Select(x => x.RoleId).ToListAsync();
        }

        public async Task<List<IdentityRole<long>>> FindRolesAsync(List<long> roleIds)
        {
            return await _context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<Patient> FindUserByEmailAsync(string email)
        {
            return await _context.Patient.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
