using MedicineProject.AuthService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicineProject.AuthService.Domain.Context
{
    public sealed class WebMobileContext : IdentityDbContext<Patient, IdentityRole<long>, long>
    {
        public DbSet<Patient> Patient { get; set; }

        public WebMobileContext(DbContextOptions<WebMobileContext> options) 
            : base(options) 
        {
        }
    }
}
