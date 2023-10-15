using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        [HttpPost("shorten")]
        public ActionResult<string> ShortenUrl([FromBody] string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
            {
                throw new ArgumentException("Original URL cannot be empty or null.");
            }

            // Validate URL format 
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!Rgx.IsMatch(originalUrl))
            {
                throw new FormatException("Original URL format is invalid.");

            }

            string encryptedUrl = Cryptography.EncryptUrl(originalUrl, _shrinkUrlSettings.MaxLength);
            string shortenedUrl = $"{_shrinkUrlSettings.BaseUrl}/{encryptedUrl}";

            return Ok(shortenedUrl);
        }
        [HttpGet("settings")]
        public ActionResult<ShrinkUrlSettings> GetUrlShortenerSettings()
        {
            return Ok(_shrinkUrlSettings);
        }

        [HttpPut("settings")]
        public ActionResult UpdateUrlShortenerSettings([FromBody] ShrinkUrlSettings newSettings)
        {
            if (newSettings == null)
            {
                return BadRequest("Invalid settings provided.");
            }

            _shrinkUrlSettings.BaseUrl = newSettings.BaseUrl;
            _shrinkUrlSettings.MaxLength = newSettings.MaxLength;
            // Optionally, add validation logic here to ensure new settings are valid.
            return Ok(_shrinkUrlSettings);
        }

        [HttpDelete("settings")]
        public ActionResult DeleteUrlShortenerSettings()
        {
            // Perform any necessary cleanup or validation before deleting the settings.
            _shrinkUrlSettings.BaseUrl = null;
            _shrinkUrlSettings.MaxLength = 0;
            return NoContent();
        }
    }
}