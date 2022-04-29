using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Data
{
    public class DataFunctions : IDataFunctions
    {
        private readonly ApplicationDbContext _context;

        public DataFunctions(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task UpdateUserCategoryEntityAsync(List<UserCategory> userCategoryItemsToDelete, List<UserCategory> userCategoryItemsToAdd)
        {
            using (var dbContextTransaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    _context.RemoveRange(userCategoryItemsToDelete);
                    await _context.SaveChangesAsync();

                    if (userCategoryItemsToAdd != null)
                    {
                        _context.AddRange(userCategoryItemsToAdd);
                        await _context.SaveChangesAsync();
                    }

                    await dbContextTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await dbContextTransaction.DisposeAsync();
                }
            }
        }

        public async Task<List<Category>> GetCategoriesThatHaveContent()
        {
            return await (from category in _context.Category
                          join categoryItem in _context.CategoryItem
                          on category.Id equals categoryItem.CategoryId
                          join content in _context.Content
                          on categoryItem.Id equals content.CategoryItem.Id
                          select new Category
                          {
                              Id = category.Id,
                              Title = category.Title,
                              Description = category.Description,

                          }).Distinct().ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesForUser(string userId, bool active)
        {
            return await (from userCategory in _context.UserCategory
                          where userCategory.UserId == userId && userCategory.Active == active
                          select new Category
                          {
                              Id = userCategory.CategoryId,

                          }).ToListAsync();
        }

        public async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
        {
            return await (from userCat in _context.UserCategory
                          where userCat.UserId == userId
                          select new UserCategory
                          {
                              Id = userCat.Id,
                              CategoryId = userCat.CategoryId,
                              UserId = userId,

                          }).ToListAsync();
        }
        public async Task<List<CategoryItemDetailsModel>> GetCategoryItemDetailsForUser(string userId, bool active)
        {
            return await (from catItem in _context.CategoryItem
                          join category in _context.Category
                          on catItem.CategoryId equals category.Id
                          join content in _context.Content
                          on catItem.Id equals content.CategoryItem.Id
                          join userCat in _context.UserCategory
                          on category.Id equals userCat.CategoryId
                          join mediaType in _context.MediaType
                          on catItem.MediaTypeId equals mediaType.Id
                          where userCat.UserId == userId && userCat.Active == active
                          select new CategoryItemDetailsModel
                          {
                              CategoryId = category.Id,
                              CategoryTitle = category.Title,
                              CategoryItemId = catItem.Id,
                              CategoryItemTitle = catItem.Title,
                              CategoryItemDescription = catItem.Description,
                              MediaImagePath = mediaType.ThumbnailImagePath,

                          }).ToListAsync();
        }
    }
}
