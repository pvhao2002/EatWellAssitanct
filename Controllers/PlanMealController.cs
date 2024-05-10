using EatWellAssistant.Models;
using EatWellAssistant.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Controllers
{
    public class PlanMealController : Controller
    {
        private readonly DBContext ctx;

        public PlanMealController(DBContext ctx)
        {
            this.ctx = ctx;
        }

        public IActionResult Index()
        {
            var viewModel = getViewModel();
            return View(viewModel);
        }
        private PlanMealViewModel getViewModel()
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            var cartByUser = ctx.Cart
                .Include(item => item.cartItems)
                .ThenInclude(item => item.food)
                .FirstOrDefault(item => item.users.userId == user.userId);
            return new PlanMealViewModel(cartByUser);
        }

        [HttpPost]
        public IActionResult Breakfast(int index)
        {
            var selectedCheckboxes = Request.Form["selection"].ToList();
            return RedirectToAction("Index");
        }

        private IActionResult updatePlanMeal(string meal, int index, List<string?> listCartItemId)
        {
            var userId = Request.Cookies["UserId"];
            var user = ctx.Users.FirstOrDefault(item => item.userId == Convert.ToInt32(string.IsNullOrEmpty(userId) ? -1 : userId));
            if (user == null)
            {
                return RedirectToAction("Index", "Authentication");
            }

            var viewModel = getViewModel();
            var dateChoose = viewModel.listDay.ToList()[index];
            var plMeal = ctx.PlanMeal
                .Include(item => item.users)
                .Include(item => item.meals)
                .ThenInclude(item => item.mealDetails)
                .FirstOrDefault(item =>
                item.users.userId == user.userId
                && item.date.Equals(dateChoose));
            var listCartItem = ctx.CartItems
                    .Where(item => listCartItemId.Contains(item.cartItemId.ToString()))
                    .ToList();
            var m = new Meal
            {
                mealName = meal,
                mealDetails = new HashSet<MealDetail>()
            };
            foreach (var item in listCartItem)
            {
                m.mealDetails.Add(new MealDetail
                {
                    foodId = item.foodId,
                    gram = item.gram ?? 0,
                    totalCalories = item.totalCalories,
                    totalProtein = item.totalProtein,
                    totalCarb = item.totalCarb,
                    totalFat = item.totalFat,
                    totalAlcohol = item.totalAlcohol
                });
            }
            m.totalCalories = listCartItem.Sum(item => item.totalCalories);
            m.totalProtein = listCartItem.Sum(item => item.totalProtein);
            m.totalCarb = listCartItem.Sum(item => item.totalCarb);
            m.totalFat = listCartItem.Sum(item => item.totalFat);
            m.totalAlcohol = listCartItem.Sum(item => item.totalAlcohol);
            if (plMeal == null)
            {
                plMeal = new Models.Entities.PlanMeal
                {
                    date = dateChoose,
                    users = user,
                    meals = new HashSet<Models.Entities.Meal> { m }
                };
                ctx.SaveChanges();
            }
            else
            {
                var mealExist = plMeal.meals.FirstOrDefault(item => meal.Equals(item.mealName));
                var listFoodId = listCartItem.Select(item => item.foodId).ToList();
                if (mealExist == null)
                {
                    plMeal.meals.Add(m);
                }

                else
                {
                    foreach (var mdetail in mealExist.mealDetails)
                    {
                        if (listFoodId.Contains(mdetail.foodId))
                        {
                            var i = listFoodId.IndexOf(mdetail.foodId);
                            mdetail.gram += listCartItem[i].gram ?? 0;
                            mdetail.totalCalories += listCartItem[i].totalCalories;
                            mdetail.totalCarb += listCartItem[i].totalCarb;
                            mdetail.totalProtein += listCartItem[i].totalProtein;
                            mdetail.totalFat += listCartItem[i].totalFat;
                            mdetail.totalAlcohol += listCartItem[i].totalAlcohol;
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}
