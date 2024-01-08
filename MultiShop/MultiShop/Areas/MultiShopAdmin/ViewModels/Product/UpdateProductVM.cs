﻿
using MultiShop.Models;
namespace MultiShop.Areas.MultiShopAdmin.ViewModels
{
    public class UpdateProductVM
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public List<int> ColorIds { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public IFormFile? HoverPhoto { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public List<int>? ImageIds { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<Category>? Categories { get; set; }    
        public List<Color>? Colors { get; set; }
    }
}
