using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;

namespace MultiShop.Areas.MultiShopAdmin.Controllers
{
    [Area("MultiShopAdmin")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        public ColorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.Include(c=>c.ProductColors).ToListAsync();
            return View(colors);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool result = _context.Colors.Any(x => x.Name == color.Name);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda color artiq movcuddur");
                return View();
            }
            Color newcolor = new Color
            {
                Name = color.Name
            };

            await _context.Colors.AddAsync(newcolor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
