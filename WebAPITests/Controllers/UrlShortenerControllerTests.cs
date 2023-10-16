using WebAPI.Controllers;
using System;
using System.Threading.Tasks;
using FluentAssertions;
using WebAPI.Model;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace WebAPI.Controllers.Tests
{
    public class UrlShortenerControllerTests
    {

        [Fact]
          public void ShortenUrlTest_ReturnOK()
        {
            //arrange
            var shrinkUrlSettings = new ShrinkUrlSettings
            {
                BaseUrl = "https://example.com/",
                MaxLength = 10
            };

            var options = Options.Create(shrinkUrlSettings);
            var controller = new UrlShortenerController(options);

            // Act
            var result = (controller.ShortenUrl("https://www.quantbe.com/welcome/canada/logs/validate")).Result as OkObjectResult;
            Assert.NotNull(result);
            var returnedResponse = result.Value as string;
            // Assert
            string expectedPrefix = shrinkUrlSettings.BaseUrl;
            int expectedLength = expectedPrefix.Length + shrinkUrlSettings.MaxLength;

            returnedResponse.Should().StartWith("https://example.com/")
                .And.HaveLength(expectedLength);


        }

        //https://stackoverflow.com/questions/73139320/unit-testing-a-controller-method-returns-null
        [Fact]
        public void ShortenUrl_WithInvalidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var shrinkUrlSettings = Options.Create(new ShrinkUrlSettings
            {
                BaseUrl = "https://example.co/",
                MaxLength = 6
            });

            var controller = new UrlShortenerController(shrinkUrlSettings);

            // Act

            // Assert
            Assert.Throws<FormatException>(() => controller.ShortenUrl("invalidurl"));

        }

        [Fact]
        public void GetUrlShortenerSettings_ReturnOK()
        {
            // Arrange
            var shrinkUrlSettings = Options.Create(new ShrinkUrlSettings
            {
                BaseUrl = "https://example.com/",
                MaxLength = 10
            });

            var controller = new UrlShortenerController(shrinkUrlSettings);

            // Act
            var result = (controller.GetUrlShortenerSettings()).Result as OkObjectResult;
            Assert.NotNull(result);
            var returnedResponse = result.Value as ShrinkUrlSettings;


            // Assert
            returnedResponse.Should().BeEquivalentTo(new ShrinkUrlSettings
            {
                BaseUrl = "https://example.com/",
                MaxLength = 10
            });
        }

        [Fact]
        public void UpdateUrlShortenerSettings_WithValidInput_ShouldReturnUpdatedSettings()
        {
            // Arrange
            var initialSettings = new ShrinkUrlSettings
            {
                BaseUrl = "https://example.co/",
                MaxLength = 6
            };

            var shrinkUrlSettings = Options.Create(initialSettings);
            var controller = new UrlShortenerController(shrinkUrlSettings);
            var newSettings = new ShrinkUrlSettings
            {
                BaseUrl = "https://updated.co/",
                MaxLength = 10
            };

            // Act
            var result = controller.UpdateUrlShortenerSettings(newSettings);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(newSettings);
        }

        [Fact]
        public void UpdateUrlShortenerSettings_WithInvalidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var initialSettings = new ShrinkUrlSettings
            {
                BaseUrl = "https://example.co/",
                MaxLength = 6
            };

            var shrinkUrlSettings = Options.Create(initialSettings);
            var controller = new UrlShortenerController(shrinkUrlSettings);
            var newSettings = new ShrinkUrlSettings
            {
                BaseUrl = null,
                MaxLength = 0
            };

            // Act
            var result = controller.UpdateUrlShortenerSettings(newSettings);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void DeleteUrlShortenerSettings_ShouldSetSettingsToDefault()
        {
            // Arrange
            var initialSettings = new ShrinkUrlSettings
            {
                BaseUrl = "https://google.co/",
                MaxLength = 6
            };

            var shrinkUrlSettings = Options.Create(initialSettings);
            var controller = new UrlShortenerController(shrinkUrlSettings);

            // Act
            var result = controller.DeleteUrlShortenerSettings();

            // Assert
            result.Should().BeOfType<NoContentResult>();
            shrinkUrlSettings.Value.Should().BeEquivalentTo(new ShrinkUrlSettings
            {
                BaseUrl = "https://example.co/",
                MaxLength = 0
            });
        }
    }
}
