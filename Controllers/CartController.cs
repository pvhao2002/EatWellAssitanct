using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
