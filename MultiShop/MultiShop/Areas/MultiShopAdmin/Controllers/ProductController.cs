using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;

namespace MultiShop.Areas.MultiShopAdmin.Controllers
{
    [Area("MultiShopAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Productlar> productlars= await _context.Productlar               
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages.Where(p=>p.IsPrimary==true))
                .ToListAsync();
            return View(productlars);
        }
    }
}
