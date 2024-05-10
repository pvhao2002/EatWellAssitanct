using EatWellAssistant.Models.Entities;

namespace EatWellAssistant.Models
{
    public class PlanMealViewModel
    {
        public ICollection<DateTime> listDay { get; set; }
        public Cart cart { get; set; }
        public PlanMealViewModel(Cart cart)
        {
            this.cart = cart;
            var today = DateTime.Now;
            this.listDay = Enumerable.Range(0, 6)
                .Select(day => today.AddDays(day)).ToList();
        }
    }
}
