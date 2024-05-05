using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewData["CurrentController"] = "Contact";
            return View();
        }
    }
}
