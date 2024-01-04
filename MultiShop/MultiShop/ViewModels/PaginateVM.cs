using MultiShop.Models;

namespace MultiShop.ViewModels
{
    public class PaginateVM<T>  where T : class,new()
    {
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<T> Items { get; set; }
        public List<Category> Categories { get; set; }
        public List<Productlar> Productlar { get; set; }
        public int? Order { get; set; }

    }
}
