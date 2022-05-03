using GoodNature.Areas.Admin.Models;
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
        private readonly IDataFunctions _dataFunctions;

        public UsersToCategoryController(ApplicationDbContext context, IDataFunctions dataFunctions)
        {
            _context = context;
            _dataFunctions = dataFunctions;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        public async Task<IActionResult> GetUsersForCategory(int categoryId)
        {
            UsersCategoryListModel usersCategoryListModel = new();

            ICollection<UserModel> allUsers = await _dataFunctions.GetAllUsers();
            ICollection<UserModel> selectedUsersForCategory = await _dataFunctions.GetSavedUsersForCategory(categoryId, false);
            ICollection<UserModel> activeUsersForCategory = await _dataFunctions.GetSavedUsersForCategory(categoryId, true);

            usersCategoryListModel.Users = allUsers;
            usersCategoryListModel.UsersSelected = selectedUsersForCategory;
            usersCategoryListModel.UsersActive = activeUsersForCategory;

            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSelectedUsers([Bind("CategoryId,UsersSelected")] UsersCategoryListModel usersCategoryListModel)
        {
            List<UserCategory> usersSelectedForCategoryToAdd = null;

            if (usersCategoryListModel.UsersSelected != null)
            {
                usersSelectedForCategoryToAdd = await GetUsersForCategoryToAdd(usersCategoryListModel);
            }

            List<UserCategory> usersSelectedForCategoryToDelete = await _dataFunctions.GetUsersForCategoryToDelete(usersCategoryListModel.CategoryId);

            await _dataFunctions.UpdateUserCategoryEntityAsync(usersSelectedForCategoryToDelete, usersSelectedForCategoryToAdd);

            usersCategoryListModel.Users = await _dataFunctions.GetAllUsers();

            return PartialView("_UsersListViewPartial", usersCategoryListModel);
        }

        private async Task<List<UserCategory>> GetUsersForCategoryToAdd(UsersCategoryListModel usersCategoryListModel)
        {
            List<UserCategory> usersForCategoryToAdd = (from userCat in usersCategoryListModel.UsersSelected
                                                        select new UserCategory
                                                        {
                                                            CategoryId = usersCategoryListModel.CategoryId,
                                                            UserId = userCat.Id,
                                         
                                                        }).ToList();
            
            return await Task.FromResult(usersForCategoryToAdd);
        }
    }
}
