using GoodNature.Data;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Controllers
{
    [Authorize]
    public class CategoriesToUserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomDataMethods _customDataMethods;

        public CategoriesToUserController(UserManager<ApplicationUser> userManager, ICustomDataMethods customDataMethods)
        {
            _userManager = userManager;
            _customDataMethods = customDataMethods;
        }

        public async Task<IActionResult> Index()
        {
            CategoriesToUserModel categoriesToUserModel = new();

            string userId = _userManager.GetUserAsync(User).Result?.Id;

            categoriesToUserModel.Categories = await _customDataMethods.GetCategoriesThatHaveContent();
            categoriesToUserModel.CategoriesSelected = await _customDataMethods.GetCategoriesForUser(userId, false);
            categoriesToUserModel.CategoriesActive = await _customDataMethods.GetCategoriesForUser(userId, true);
            categoriesToUserModel.UserId = userId;

            return View(categoriesToUserModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string[] categoriesSelected, string[] categoriesActive)
        {
            string userId = _userManager.GetUserAsync(User).Result?.Id;

            List<UserCategory> usersCategoriesToDelete = await _customDataMethods.GetCategoriesToDeleteForUser(userId);
            List<UserCategory> usersCategoriesToAdd = GetCategoriesToAddForUser(categoriesSelected, categoriesActive, userId);

            await _customDataMethods.UpdateUserCategoryEntityAsync(usersCategoriesToDelete, usersCategoriesToAdd);

            return RedirectToAction("Index", "Home");
        }

        private List<UserCategory> GetCategoriesToAddForUser(string[] categoriesSelected, string[] categoriesActive, string userId)
        {
            IEnumerable<string> categoriesSelectedAndActive = categoriesSelected.Union(categoriesActive);

            return (from categoryId in categoriesSelectedAndActive
                    select new UserCategory
                    {
                        UserId = userId,
                        CategoryId = int.Parse(categoryId),
                        Active = categoriesActive.Contains(categoryId),

                    }).ToList();
        }
    }
}
