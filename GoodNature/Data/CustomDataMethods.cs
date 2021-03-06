using GoodNature.Areas.Admin.Models;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Data
{
    public class CustomDataMethods : ICustomDataMethods
    {
        private readonly ApplicationDbContext _context;

        public CustomDataMethods(ApplicationDbContext context)
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

        public async Task RelateCategoryToUser(string userId, int categoryId)
        {
            var userCategory = new UserCategory
            {
                UserId = userId,
                CategoryId = categoryId,
            };

            _context.UserCategory.Add(userCategory);

            await _context.SaveChangesAsync();
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
                              ThumbnailImagePath = category.ThumbnailImagePath,

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
        
        public async Task<List<CategoryItem>> GetCategoryItemList(int categoryId)
        {
            return await (from catItem in _context.CategoryItem
                          join contentItem in _context.Content
                          on catItem.Id equals contentItem.CategoryItem.Id
                          into gj
                          from subContent in gj.DefaultIfEmpty()
                          where catItem.CategoryId == categoryId
                          select new CategoryItem
                          {
                              Id = catItem.Id,
                              Title = catItem.Title,
                              Description = catItem.Description,
                              DateTimeItemReleased = catItem.DateTimeItemReleased,
                              MediaTypeId = catItem.MediaTypeId,
                              CategoryId = categoryId,
                              ContentId = (subContent != null) ? subContent.Id : 0,

                          }).ToListAsync();
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await (from user in _context.Users
                          select new UserModel
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                              FirstName = user.FirstName,
                              LastName = user.LastName,

                          }).ToListAsync();
        }

        public async Task<List<UserModel>> GetSavedUsersForCategory(int categoryId, bool active)
        {
            return await (from usersToCat in _context.UserCategory
                          where usersToCat.CategoryId == categoryId && usersToCat.Active == active
                          select new UserModel
                          {
                              Id = usersToCat.UserId,

                          }).ToListAsync();
        }

        public async Task<List<UserCategory>> GetUsersForCategoryToDelete(int categoryId)
        {
            return await (from userCat in _context.UserCategory
                          where userCat.CategoryId == categoryId
                          select new UserCategory
                          {
                              Id = userCat.Id,
                              CategoryId = categoryId,
                              UserId = userCat.UserId,

                          }).ToListAsync();
        }
    }
}
