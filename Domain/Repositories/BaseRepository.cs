using MedicineProject.AuthService.Domain.Context;
using MedicineProject.AuthService.Domain.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedicineProject.AuthService.Domain.Repositories
{
    public class BaseRepository : IBaseRepository 
    {
        protected readonly WebMobileContext _context;
        
        public BaseRepository(WebMobileContext context)
        {
            _context = context;
        }

        public async Task<EntityEntry<TModel>> CreateItemAsync<TModel>(TModel item)
            where TModel : BaseModel
        {
            var itemFromDb = await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();
            return itemFromDb;
        }

        public async Task DeleteItemAsync<TModel>(long id)
            where TModel : BaseModel
        {
            TModel oldItem = await _context.Set<TModel>().FindAsync(id);
            if( oldItem != null)
            {
                _context.Set<TModel>().Remove(oldItem);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<TModel>> GetItemListAsync<TModel>()
            where TModel : BaseModel
        {
            return await _context.Set<TModel>().ToListAsync();
        }

        public async Task<TModel> TryGetItemByIdAsync<TModel>(long id)
            where TModel : BaseModel
        {
            return await _context.Set<TModel>().FindAsync(id);
        }

        public async Task<TModel> TryGetItemByNameAsync<TModel>(string name)
            where TModel : BaseModel
        {
            return await _context.Set<TModel>().FirstOrDefaultAsync(item => item.Name.Contains(name));
        }

        public async Task<EntityEntry<TModel>> UpdateItemAsync<TModel>(TModel item, TModel oldItem)
            where TModel : BaseModel
        {
            EntityEntry<TModel> updatedItem = _context.Set<TModel>().Update(oldItem);
            await _context.SaveChangesAsync();
            return updatedItem;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
