using GoodNature.Data;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryItemDetailsModel> selectedCategoryItemDetailsModels = null;
            IEnumerable<CategoryItemDetailsModel> activeCategoryItemDetailsModels = null;

            IEnumerable<GroupedCategoryItemsModel> selectedGroupedCategoryItemsModels = null;
            IEnumerable<GroupedCategoryItemsModel> activeGroupedCategoryItemsModels = null;

            CategoryDetailsModel categoryDetailsModel = new();

            if (_signInManager.IsSignedIn(User))
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                if(user != null)
                {
                    selectedCategoryItemDetailsModels = await GetSelectedCategoryItemDetailsForUser(user.Id);
                    activeCategoryItemDetailsModels = await GetActiveCategoryItemDetailsForUser(user.Id);

                    selectedGroupedCategoryItemsModels = GetGroupedCategoryItemsModel(selectedCategoryItemDetailsModels);
                    activeGroupedCategoryItemsModels = GetGroupedCategoryItemsModel(activeCategoryItemDetailsModels);

                    categoryDetailsModel.SelectedGroupedCategoryItemsModels = selectedGroupedCategoryItemsModels;
                    categoryDetailsModel.ActiveGroupedCategoryItemsModels = activeGroupedCategoryItemsModels;
                }
            }
            else
            {
                IEnumerable<Category> categoriesWithContent = await GetCategoriesWithContent();

                categoryDetailsModel.Categories = categoriesWithContent;
            }
            
            return View(categoryDetailsModel);
        }

        private async Task<List<CategoryItemDetailsModel>> GetSelectedCategoryItemDetailsForUser(string userId)
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
                          where userCat.UserId == userId && userCat.Active == false
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

        private async Task<List<CategoryItemDetailsModel>> GetActiveCategoryItemDetailsForUser(string userId)
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
                          where userCat.UserId == userId && userCat.Active == true
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

        private IEnumerable<GroupedCategoryItemsModel> GetGroupedCategoryItemsModel(
            IEnumerable<CategoryItemDetailsModel> categoryItemDetailsModels)
        {
            return from item in categoryItemDetailsModels
                   group item by item.CategoryId into g
                   select new GroupedCategoryItemsModel
                   {
                       Id = g.Key,
                       Title = g.Select(c => c.CategoryTitle).FirstOrDefault(),
                       Items = g,
                   };
        }

        private async Task<List<Category>> GetCategoriesWithContent()
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
