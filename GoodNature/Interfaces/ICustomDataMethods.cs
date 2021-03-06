using GoodNature.Areas.Admin.Models;
using GoodNature.Entities;
using GoodNature.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoodNature.Data
{
    public interface ICustomDataMethods
    {
        Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, List<UserCategory> userCategoryItemsToAdd);

        Task RelateCategoryToUser(string userId, int categoryId);

        Task<List<Category>> GetCategoriesThatHaveContent();

        Task<List<Category>> GetCategoriesForUser(string userId, bool active);

        Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId);

        Task<List<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId, bool active);

        Task<List<CategoryItem>> GetCategoryItemList(int categoryId);

        Task<List<UserModel>> GetAllUsers();

        Task<List<UserModel>> GetSavedUsersForCategory(int categoryId, bool active);

        Task<List<UserCategory>> GetUsersForCategoryToDelete(int categoryId);
    }
}
