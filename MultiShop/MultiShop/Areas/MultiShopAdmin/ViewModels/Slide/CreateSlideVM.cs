using System.ComponentModel.DataAnnotations;

namespace MultiShop.Areas.MultiShopAdmin.ViewModels
{
    public class CreateSlideVM
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
