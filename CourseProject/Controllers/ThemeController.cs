using CourseProject_Utility;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers
{
    public class ThemeController : ControllerBase
    {
        public ActionResult ChangeTheme()
        {
            if (Request.Cookies["theme"] == null)
            {
                Response.Cookies.Append("theme", "dark");
            }
            else
            {
                if (Request.Cookies["theme"]== "dark")
                {
                    Response.Cookies.Append("theme", "light");
                }
                else if (Request.Cookies["theme"] == "light")
                {
                    Response.Cookies.Append("theme", "dark");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
