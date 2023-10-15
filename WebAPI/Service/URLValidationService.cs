using System.Text.RegularExpressions;

namespace WebAPI.Service
{
    public static class URLValidationService
    {
        
        public static bool ValidateURL(string originalUrl)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(originalUrl);

        }
    }
}
