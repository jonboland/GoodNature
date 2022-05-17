using FakeItEasy;
using GoodNature.Controllers;
using GoodNature.Data;
using GoodNature.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GoodNature.Tests.Unit
{
    public class UserAuthControllerTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICustomDataMethods _customDataMethods;

        public UserAuthControllerTests()
        {
            _signInManager = A.Fake<SignInManager<ApplicationUser>>();
            _userManager = A.Fake<UserManager<ApplicationUser>>();
            _customDataMethods = A.Fake<ICustomDataMethods>();
        }

        [Fact]
        public async void Login_ReturnsPartialViewWithLoginInvalid_WhenModelStateInvalid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UserAuthController_Login")
            .Options;
            using var context = new ApplicationDbContext(options);
            var userAuthController = new UserAuthController(_userManager, _signInManager, context, _customDataMethods);
            userAuthController.ModelState.AddModelError("ErrorKey", "ErrorMessage");
            var loginModel = new LoginModel();

            // Act
            var result = await userAuthController.Login(loginModel);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsAssignableFrom<LoginModel>(partialViewResult.ViewData.Model);
            Assert.Equal("_UserLoginPartial", partialViewResult.ViewName);
            Assert.Equal("true", model.LoginInvalid);
        }

        [Fact]
        public async void Login_SetsLoginValid_WhenSignInSucceeds()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserAuthController_LoginValid")
                .Options;
            using var context = new ApplicationDbContext(options);
            var userAuthController = new UserAuthController(_userManager, _signInManager, context, _customDataMethods);
            var loginModel = new LoginModel();
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Success;
            A.CallTo(() => _signInManager.PasswordSignInAsync(A<string>._, A<string>._, A<bool>._, A<bool>._))
                .Returns(Task.FromResult(signInResult));

            // Act
            var result = await userAuthController.Login(loginModel);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            var model = Assert.IsAssignableFrom<LoginModel>(partialViewResult.ViewData.Model);
            Assert.Equal(string.Empty, model.LoginInvalid);
        }

        [Fact]
        public async void Login_AddsError_WhenSignInFails()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserAuthController_LoginError")
                .Options;
            using var context = new ApplicationDbContext(options);
            var userAuthController = new UserAuthController(_userManager, _signInManager, context, _customDataMethods);
            var loginModel = new LoginModel();
            var signInResult = Microsoft.AspNetCore.Identity.SignInResult.Failed;
            A.CallTo(() => _signInManager.PasswordSignInAsync(A<string>._, A<string>._, A<bool>._, A<bool>._))
                .Returns(Task.FromResult(signInResult));

            // Act
            var result = await userAuthController.Login(loginModel);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal(
                Constants.LoginFailedErrorMessage,
                partialViewResult.ViewData.ModelState[string.Empty].Errors.First().ErrorMessage);
        }

        [Theory]
        [InlineData("test@test.com", true)]
        [InlineData("Test@Test.com", true)]
        [InlineData("TEST@TEST.com", true)]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("other@test.com", false)]
        public async void UserNameExists_ReturnsCorrectBoolValue(string userName, bool expected)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("UserAuthController_UserNameExists")
                .Options;
            using var context = new ApplicationDbContext(options);
            context.Users.Add(new ApplicationUser { UserName = "test@test.com" });
            context.SaveChanges();
            var userAuthController = new UserAuthController(_userManager, _signInManager, context, _customDataMethods);

            // Act
            var actual = await userAuthController.UserNameExists(userName);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
