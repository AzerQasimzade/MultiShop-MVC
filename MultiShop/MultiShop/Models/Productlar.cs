﻿using System.Reflection.Metadata.Ecma335;

namespace MultiShop.Models
{
    public class Productlar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? Image { get; set; }//bunu category-leri önceden yazdığım üçün saxlamışam
        public decimal SalePrice { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<ProductColor>? ProductColors { get; set; }
    }
}
