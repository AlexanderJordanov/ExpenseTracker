using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ThemeController : Controller
    {
        public IActionResult Set(string theme, string? returnUrl = "/")
        {
            // позволяваме само нашите 3 теми
            var allowedThemes = new[] { "lux", "cosmo", "darkly" };

            if (!allowedThemes.Contains(theme))
            {
                theme = "lux";
            }

            Response.Cookies.Append("theme", theme, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true
            });

            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
    }
}
