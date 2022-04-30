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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataFunctions _dataFunctions;

        public HomeController(
            ILogger<HomeController> logger,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IDataFunctions dataFunctions)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _dataFunctions = dataFunctions;
        }

        public async Task<IActionResult> Index()
        {
            CategoryDetailsModel categoryDetailsModel = new();

            if (_signInManager.IsSignedIn(User))
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                if(user != null)
                {
                    IEnumerable<CategoryItemDetailsModel> selectedCategoryItemDetailsModels = await _dataFunctions.GetCategoryItemDetailsForUser(user.Id, false);
                    IEnumerable<CategoryItemDetailsModel> activeCategoryItemDetailsModels = await _dataFunctions.GetCategoryItemDetailsForUser(user.Id, true);

                    IEnumerable<GroupedCategoryItemsModel> groupedSelectedCategoryItemsModels = GetGroupedCategoryItemsModels(selectedCategoryItemDetailsModels);
                    IEnumerable<GroupedCategoryItemsModel> groupedActiveCategoryItemsModels = GetGroupedCategoryItemsModels(activeCategoryItemDetailsModels);

                    categoryDetailsModel.GroupedSelectedCategoryItemsModels = groupedSelectedCategoryItemsModels;
                    categoryDetailsModel.GroupedActiveCategoryItemsModels = groupedActiveCategoryItemsModels;
                }
            }
            else
            {
                IEnumerable<Category> categoriesWithContent = await _dataFunctions.GetCategoriesThatHaveContent();

                categoryDetailsModel.Categories = categoriesWithContent;
            }
            
            return View(categoryDetailsModel);
        }

        private IEnumerable<GroupedCategoryItemsModel> GetGroupedCategoryItemsModels(
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
