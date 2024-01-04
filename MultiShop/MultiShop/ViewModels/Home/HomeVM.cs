using MultiShop.Models;

namespace MultiShop.ViewModels.Home
{
    public class HomeVM
    {
        public List<Slide> Slides { get; set; }
        public List<Category> Categories { get; set; }
        public List<Productlar> Productlar { get; set; }
        public List<Productlar> RecentProducts { get; set; }
        public int? Order { get; set; }




    }
}
