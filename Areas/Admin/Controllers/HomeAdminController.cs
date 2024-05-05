using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
