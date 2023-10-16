using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using System;
using System.Security.Policy;
using System.Text.RegularExpressions;
using WebAPI.Model;
using WebAPI.Service;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly ShrinkUrlSettings _shrinkUrlSettings;

        public UrlShortenerController(IOptions<ShrinkUrlSettings> shrinkUrlSettings)
        {
            _shrinkUrlSettings = shrinkUrlSettings.Value;
        }

        [HttpPost("generateurl")]
        public ActionResult<string> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be empty or null.");
            }

            // Validate URL format 
            
            if (!URLValidationService.ValidateURL(originalUrl))
            {
                throw new FormatException("Original URL format is invalid.");

            }

            string encryptedUrl = CryptographyService.EncryptUrl(originalUrl, _shrinkUrlSettings.MaxLength);
            string shortenedUrl = $"{_shrinkUrlSettings.BaseUrl}{encryptedUrl}";
            Console.WriteLine(Ok(shortenedUrl));
            return Ok(shortenedUrl);
        }

        [HttpGet("settings")]
        public ActionResult<ShrinkUrlSettings> GetUrlShortenerSettings()
        {
            Console.WriteLine(Ok(_shrinkUrlSettings));

            return Ok(_shrinkUrlSettings);
        }

        [HttpPut("settings")]
        public ActionResult UpdateUrlShortenerSettings([FromBody] ShrinkUrlSettings newSettings)
        {
            if (newSettings == null)
            {
                return BadRequest("Invalid settings provided.");
            }

            if (newSettings.BaseUrl == null || !URLValidationService.ValidateURL(newSettings.BaseUrl))
            {
                return BadRequest("Invalid base URL provided.");

            }
            if (newSettings.MaxLength < 1)
            {
                return BadRequest("Invalid max length provided.");

            }
            _shrinkUrlSettings.BaseUrl = newSettings.BaseUrl;
            _shrinkUrlSettings.MaxLength = newSettings.MaxLength;
            
            return Ok(_shrinkUrlSettings);
        }

        [HttpDelete("settings")]
        public ActionResult DeleteUrlShortenerSettings()
        {
            // Perform any necessary cleanup or validation before deleting the settings.
            _shrinkUrlSettings.BaseUrl = "https://example.co/";
            _shrinkUrlSettings.MaxLength = 0;
            return NoContent();
        }
    }
}