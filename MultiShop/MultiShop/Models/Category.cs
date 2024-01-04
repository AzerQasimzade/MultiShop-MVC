using System.ComponentModel.DataAnnotations;

namespace MultiShop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Productlar>? Productlar { get; set; }
        public string? Image { get; set; }
        public int ProductCount { get; set; }

    }
}
