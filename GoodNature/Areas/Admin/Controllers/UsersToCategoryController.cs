using GoodNature.Areas.Admin.Models;
using GoodNature.Comparers;
using GoodNature.Data;
using GoodNature.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNature.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersToCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICustomDataMethods _customDataMethods;

        public UsersToCategoryController(ApplicationDbContext context, ICustomDataMethods customDataMethods)
        {
            _context = context;
            _customDataMethods = customDataMethods;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UsersCategoryListModel usersCategoryListModel = new();

            usersCategoryListModel.Users = await _customDataMethods.GetAllUsers();
            usersCategoryListModel.UsersSelected = await _customDataMethods.GetSavedUsersForCategory(categoryId, false);
            usersCategoryListModel.UsersActive = await _customDataMethods.GetSavedUsersForCategory(categoryId, true);

            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId,UsersSelected,UsersActive")] UsersCategoryListModel usersCategoryListModel)
        {
            List<UserCategory> usersForCategoryToAdd = null;

            if (usersCategoryListModel.UsersActive != null || usersCategoryListModel.UsersSelected != null)
            {
                usersForCategoryToAdd = GetUsersForCategoryToAdd(usersCategoryListModel);
            }

            List<UserCategory> usersSelectedForCategoryToDelete = await _customDataMethods.GetUsersForCategoryToDelete(usersCategoryListModel.CategoryId);

            await _customDataMethods.UpdateUserCategoryEntityAsync(usersSelectedForCategoryToDelete, usersForCategoryToAdd);

            usersCategoryListModel.Users = await _customDataMethods.GetAllUsers();

            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }

        private List<UserCategory> GetUsersForCategoryToAdd(UsersCategoryListModel usersCategoryListModel)
        {
            List<UserCategory> usersActiveForCategoryToAdd = new();
            List<UserCategory> usersSelectedForCategoryToAdd = new();

            if (usersCategoryListModel.UsersActive != null)
            {
                usersActiveForCategoryToAdd = (from userCat in usersCategoryListModel.UsersActive
                                               select new UserCategory
                                               {
                                                   CategoryId = usersCategoryListModel.CategoryId,
                                                   UserId = userCat.Id,
                                                   Active = true,

                                               }).ToList();
            }

            if (usersCategoryListModel.UsersSelected != null)
            {
                usersSelectedForCategoryToAdd = (from userCat in usersCategoryListModel.UsersSelected
                                                 select new UserCategory
                                                 {
                                                     CategoryId = usersCategoryListModel.CategoryId,
                                                     UserId = userCat.Id,
                                                     Active = false,

                                                 }).ToList();
            }

            return usersActiveForCategoryToAdd.Union(usersSelectedForCategoryToAdd, new CompareUserCategories()).ToList();
        }
    }
}
