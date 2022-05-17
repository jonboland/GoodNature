using System;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;

namespace GoodNature.Tests.Integration
{
    public class HomeControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient httpClient;

        public HomeControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            httpClient = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Home/Privacy")]
        public async void Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var expectedContentType = "text/html; charset=utf-8";

            // Act
            var response = await httpClient.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode();
            var contentType = response.Content.Headers.ContentType.ToString();
            Assert.Equal(expectedContentType, contentType);
        }

        [Fact]
        public async Task Index_SectionsArePresent_WhenSignedOut()
        {
            // Arrange
            var address = "/Home/Index";
            var about = "About Us";
            var contact = "Contact Us";          

            // Act
            var response = await httpClient.GetAsync(address);
            var pageContent = await response.Content.ReadAsStringAsync();

            // Assert
            pageContent.Should().Contain(about).And.Contain(contact);
        }
    }
}
