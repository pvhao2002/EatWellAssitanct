using EatWellAssistant.Models;
using Microsoft.AspNetCore.Mvc;

namespace EatWellAssistant.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly DBContext ctx;

        public HomeAdminController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var totalP = ctx.Food.Where(item => item.status == true).ToList().Count;
            var totalO = ctx.Users.ToList().Count;
            var totalR = ctx.PlanMeal.ToList().Count;
            var homeModel = new HomeAdminViewModel(totalP, totalO, totalR);
            return View(homeModel);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetInt32("user_id", 0);
            HttpContext.Session.SetString("userFullName", string.Empty);
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
