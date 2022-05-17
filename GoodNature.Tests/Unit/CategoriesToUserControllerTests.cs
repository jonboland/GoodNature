using FakeItEasy;
using FluentAssertions;
using GoodNature.Controllers;
using GoodNature.Data;
using GoodNature.Entities;
using GoodNature.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoodNature.Tests.Unit
{
    public class CategoriesToUserControllerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomDataMethods _customDataMethods;

        private CategoriesToUserController _categoriesToUserController;

        public CategoriesToUserControllerTests()
        {
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _customDataMethods = A.Fake<ICustomDataMethods>();
            _categoriesToUserController = new CategoriesToUserController(_userManager, _customDataMethods);
        }

        [Fact]
        public async void Index_CallsMethodsAndReturnsViewResult()
        {

            // Arrange
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>.Ignored)).Returns(new ApplicationUser());
            A.CallTo(() => _customDataMethods.GetCategoriesThatHaveContent()).Returns(new List<Category>());
            A.CallTo(() => _customDataMethods.GetCategoriesForUser(A<string>.Ignored, A<bool>.Ignored))
                .Returns(new List<Category>());

            // Act
            var result = await _categoriesToUserController.Index();

            // Assert
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => _customDataMethods.GetCategoriesForUser(A<string>.Ignored, A<bool>.Ignored))
                .MustHaveHappenedTwiceExactly();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async void Index_ViewResultContainsData()
        {
            // Arrange
            var t1 = "Protect the Bumblebee";
            var t2 = "Save the Ant";
            var t3 = "Woodland Care";
            A.CallTo(() => _userManager.GetUserAsync(A<ClaimsPrincipal>.Ignored))
                .Returns(new ApplicationUser() { Id = Guid.NewGuid().ToString() });
            A.CallTo(() => _customDataMethods.GetCategoriesThatHaveContent())
                .Returns(new List<Category>
                {
                    new Category() { Title = t1 },
                    new Category() { Title = t2 },
                    new Category() { Title = t3 },
                });
            A.CallTo(() => _customDataMethods.GetCategoriesForUser(A<string>.Ignored, false))
                .Returns(new List<Category> { new Category() { Title = t1 } });
            A.CallTo(() => _customDataMethods.GetCategoriesForUser(A<string>.Ignored, true))
                .Returns(new List<Category> { new Category() { Title = t2 }, new Category() { Title = t3 } });

            // Act
            var result = await _categoriesToUserController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoriesToUserModel>(viewResult.ViewData.Model);
            model.UserId.Should().HaveLength(36);
            model.Categories.Should().HaveCount(3).And.OnlyHaveUniqueItems();
            model.CategoriesSelected.Should().HaveCount(1).And.Contain(cs => cs.Title == t1);
            model.CategoriesActive.Should().HaveCount(2).And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async void IndexPost_RedirectsToHomePage()
        {
            // Arrange
            string[] categoriesSelected = { "1" };
            string[] categoriesActive = { "2" };
            A.CallTo(() => _userManager.GetUserAsync(
                A<ClaimsPrincipal>.Ignored)).Returns(new ApplicationUser());
            A.CallTo(() => _customDataMethods.GetCategoriesToDeleteForUser(
                A<string>.Ignored)).Returns(new List<UserCategory>());
            A.CallTo(() => _customDataMethods.UpdateUserCategoryEntityAsync(
                A<List<UserCategory>>.Ignored, A<List<UserCategory>>.Ignored)).DoesNothing();

            // Act
            var result = await _categoriesToUserController.Index(categoriesSelected, categoriesActive);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }
    }
}
