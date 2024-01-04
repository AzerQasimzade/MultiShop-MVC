using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.ViewModels.Home;

namespace MultiShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides=await _context.Slides.OrderBy(s => s.Order).Take(3).ToListAsync();
            List<Category> categories =await _context.Categories.ToListAsync();
            List<Productlar> productlar =await _context.Productlar.Take(6).Include(p=>p.ProductImages).ToListAsync();
            List<Productlar> recentProducts=await _context.Productlar.OrderByDescending(s=>s.Id).Take(6).Include(p => p.ProductImages).ToListAsync();

            HomeVM homeVM = new HomeVM
            {
                Slides = slides,
                Categories = categories,
                Productlar = productlar,
                RecentProducts = recentProducts,
                
            };
            return View(homeVM);
        }
    }
}
