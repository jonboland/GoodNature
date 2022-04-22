using GoodNature.Data;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private IDataFunctions _dataFunctions;

        public CategoriesToUserController(
            ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDataFunctions dataFunctions)
        {
            _context = context;
            _userManager = userManager;
            _dataFunctions = dataFunctions;
        }

        public async Task<IActionResult> Index()
        {
            CategoriesToUserModel categoriesToUserModel = new();

            string userId = _userManager.GetUserAsync(User).Result?.Id;

            categoriesToUserModel.Categories = await GetCategoriesThatHaveContent();

            categoriesToUserModel.CategoriesSelected = await GetCategoriesCurrentlySavedForUser(userId);

            categoriesToUserModel.UserId = userId;

            return View(categoriesToUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected)
        {
            string userId = _userManager.GetUserAsync(User).Result?.Id;

            List<UserCategory> usersCategoriesToDelete = await GetCategoriesToDeleteForUser(userId);

            List<UserCategory> usersCategoriesToAdd = GetCategoriesToAddForUser(categoriesSelected, userId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(usersCategoriesToDelete, usersCategoriesToAdd);

            return RedirectToAction("Index", "Home");
        }

        private async Task<List<Category>> GetCategoriesThatHaveContent()
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

        private async Task<List<Category>> GetCategoriesCurrentlySavedForUser(string userId)
        {
            return await (from userCategory in _context.UserCategory
                          where userCategory.UserId == userId
                          select new Category
                          {
                              Id = userCategory.CategoryId,
                          }).ToListAsync();           
        }

        private async Task<List<UserCategory>> GetCategoriesToDeleteForUser(string userId)
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

        private List<UserCategory> GetCategoriesToAddForUser(string[] categoriesSelected, string userId)
        {
            return (from categoryId in categoriesSelected
                    select new UserCategory
                    {
                        UserId = userId,
                        CategoryId = int.Parse(categoryId),
                    }).ToList();
        }
    }
}
