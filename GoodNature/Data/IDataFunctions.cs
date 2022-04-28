using GoodNature.Entities;
using GoodNature.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNature.Data
{
    public interface IDataFunctions
    {
        Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, List<UserCategory> userCategoryItemsToAdd);

        Task<List<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId, bool active);
    }
}
