using MultiShop.Models;
using System.ComponentModel.DataAnnotations;

namespace MultiShop.Areas.MultiShopAdmin.ViewModels
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public List<int> TagIds { get; set; }
        public List<int> ColorIds { get; set; }
        public List<int> SizeIds { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Color>? Colors { get; set; }
        public IFormFile MainPhoto { get; set; }
        public IFormFile HoverPhoto { get; set; }
        public List<IFormFile>? Photos { get; set; }


    }
}
