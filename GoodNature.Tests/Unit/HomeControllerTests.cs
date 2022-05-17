using FakeItEasy;
using FluentAssertions;
using GoodNature.Controllers;
using GoodNature.Data;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace GoodNature.Tests.Unit
{
    public class HomeControllerTests
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomDataMethods _customDataMethods;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            _logger = A.Fake<ILogger<HomeController>>();
            _signInManager = A.Fake<SignInManager<ApplicationUser>>();
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _customDataMethods = A.Fake<ICustomDataMethods>();
            _homeController = new HomeController(_logger, _signInManager, _userManager, _customDataMethods);
        }

        [Fact]
        public async void Index_CallsMethodsAndReturnsViewResult_WhenUserSignedOut()
        {
            // Arrange
            A.CallTo(() => _signInManager.IsSignedIn(A<ClaimsPrincipal>.Ignored)).Returns(false);
            A.CallTo(() => _customDataMethods.GetCategoriesThatHaveContent()).Returns(new List<Category>());

            // Act
            var result = await _homeController.Index();

            // Assert
            A.CallTo(() => _signInManager.IsSignedIn(A<ClaimsPrincipal>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _customDataMethods.GetCategoriesThatHaveContent()).MustHaveHappenedOnceExactly();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ViewResultContainsGroupedCategoryItemModels_WhenUserSignedIn()
        {
            // Arrange
            string t1 = "Protect the Bumblebee";
            string t2 = "Save the Wood Ant";
            var catItemDetailsModels = new List<CategoryItemDetailsModel>
            {
                new CategoryItemDetailsModel { CategoryId = 1, CategoryItemId = 2, CategoryTitle = t1 },
                new CategoryItemDetailsModel { CategoryId = 2, CategoryItemId = 7, CategoryTitle = t2 },
                new CategoryItemDetailsModel { CategoryId = 1, CategoryItemId = 4, CategoryTitle = t1 },
                new CategoryItemDetailsModel { CategoryId = 2, CategoryItemId = 9, CategoryTitle = t2 },
                new CategoryItemDetailsModel { CategoryId = 1, CategoryItemId = 6, CategoryTitle = t1 },
                new CategoryItemDetailsModel { CategoryId = 2, CategoryItemId = 1009, CategoryTitle = t2 },
            };
            A.CallTo(() => _signInManager.IsSignedIn(A<ClaimsPrincipal>.Ignored)).Returns(true);
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>.Ignored)).Returns(new ApplicationUser());
            A.CallTo(() => _customDataMethods.GetCategoryItemDetailsForUser(A<string>.Ignored, A<bool>.Ignored))
                .Returns(catItemDetailsModels);

            // Act
            var result = await _homeController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryDetailsModel>(viewResult.ViewData.Model);
            var groupedCatItemsModels = model.GroupedActiveCategoryItemsModels;
            groupedCatItemsModels.Should().HaveCount(2).And.OnlyHaveUniqueItems();
            groupedCatItemsModels.Should().Contain(g => g.Title == t1).And.Contain(g => g.Title == t2);
            groupedCatItemsModels.Should().Contain(g => g.Items.Count() == 3);
        }
    }
}
