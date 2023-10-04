using MedicineProject.AuthService.Domain.Models.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MedicineProject.AuthService.Domain.Repositories
{
    public interface IBaseRepository
    {
        Task<List<TModel>> GetItemListAsync<TModel>() where TModel : BaseModel;

        Task<TModel> TryGetItemByIdAsync<TModel>(long id) where TModel : BaseModel;

        Task<TModel> TryGetItemByNameAsync<TModel>(string name) where TModel : BaseModel;

        Task<EntityEntry<TModel>> CreateItemAsync<TModel>(TModel item) where TModel : BaseModel;

        Task<EntityEntry<TModel>> UpdateItemAsync<TModel>(TModel item, TModel oldItem) where TModel : BaseModel; 

        Task DeleteItemAsync<TModel>(long id) where TModel : BaseModel;

        Task SaveChangesAsync();
    }
}
