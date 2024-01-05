using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;

namespace MultiShop.Areas.MultiShopAdmin.Controllers
{
    [Area("MultiShopAdmin")]

    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
           List<Category> categories= await _context.Categories.Include(p=>p.Productlar).ToListAsync();

           return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            } 
            bool result =  _context.Categories.Any(c => c.Name == category.Name);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda category artiq movcuddur");
                return View();
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category existed = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            bool result = await _context.Categories.AnyAsync(x => x.Name == category.Name && x.Id != category.Id);
            if (result)
            {
                ModelState.AddModelError("Name", "We have Same Category Name.Please Try Another Name");
                return View();
            }
            existed.Name = category.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Category existed = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            _context.Categories.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
