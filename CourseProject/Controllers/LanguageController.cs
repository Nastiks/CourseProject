using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    [Route("languages")]
    public class LanguageController : Controller
    {
        [Route("change")]
        public IActionResult Change(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMonths(1)});
            return RedirectToAction("Index", "Home");
        }
    }
}
