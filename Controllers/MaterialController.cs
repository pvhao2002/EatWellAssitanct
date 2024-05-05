using EatWellAssistant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace EatWellAssistant.Controllers
{
    public class MaterialController : Controller
    {
        private readonly DBContext ctx;

        public MaterialController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            var listProduct = ctx.Food.Include(item => item.category).ToPagedList(page ?? 1, pageSize);
            return View(listProduct);
        }
    }
}
