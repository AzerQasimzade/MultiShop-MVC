using System.ComponentModel.DataAnnotations;

namespace MultiShop.Models
{
    public class Category
    {
        public int Id { get; set; } 
        [Required(ErrorMessage = "Ad mutleq daxil edilmelidir")]
        [MaxLength(25, ErrorMessage = "25 den uzun deyer gonderilmemelidir")]
        public string Name { get; set; }
        public List<Productlar>? Productlar { get; set; }
        public string? Image { get; set; }
        public int ProductCount { get; set; }
    }
}
