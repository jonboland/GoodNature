using System.Linq;
using FluentAssertions;
using GoodNature.Controllers;
using GoodNature.Data;
using GoodNature.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Xunit;

namespace GoodNature.Tests.Unit
{
    public class ContentControllerTests
    {
        [Fact]
        public async void Index_ReturnsViewResultWithContentRelatedToCategoryItem()
        {
            // Arrange
            var item1 = "Bumblebee Item";
            var item2 = "Ant Item";
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("ContentController_Index")
                .Options;
            using var context = new ApplicationDbContext(options);
            context.Content.AddRange(
                new Content { CategoryItem = new CategoryItem { Id = 1, Title = item1 } },
                new Content { CategoryItem = new CategoryItem { Id = 2, Title = item2 } });
            context.SaveChanges();
            var contentController = new ContentController(context);

            // Act
            var result = await contentController.Index(2);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Content>(viewResult.ViewData.Model);
            model.CategoryItem.Title.Should().Be(item2);
        }
    }
}
