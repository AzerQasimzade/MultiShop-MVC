using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.ViewModels;
using MultiShop.ViewModels.Detail;
using MultiShop.ViewModels.Home;

namespace MultiShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Shop(int page=1,int order=1)
        {
            int count= await _context.Productlar.CountAsync();

            List<Productlar> productlars = await _context.Productlar.Skip((page-1)*6).Take(6)
                .Include(c=>c.Category) 
                .ToListAsync();
            switch (order)
            {
                case 1:
                    productlars = productlars.OrderBy(p => p.Name).ToList();
                    break;
                case 2:
                    productlars = productlars.OrderBy(p => p.Price).ToList();
                    break;
                case 3:
                    productlars = productlars.OrderByDescending(p => p.Id).ToList();
                    break; 
            }
            
            PaginateVM<Productlar> paginateVM = new PaginateVM<Productlar>
            {
                Categories = await _context.Categories.Include(c => c.Productlar).ToListAsync(),
                Productlar = productlars,        
                Order = order,
                Items = productlars,
                CurrentPage = page,
                TotalPage= Math.Ceiling((double)count / 6)
            };
            return View(paginateVM);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id<=0) return BadRequest();
            Productlar product=await _context.Productlar
                .Include(p=>p.Category)
                .Include(p => p.ProductImages)
                .Include(p=>p.ProductColors)
                .ThenInclude(p=>p.Color)
                .FirstOrDefaultAsync(p => p.Id == id);

            List<Productlar> relatedproducts = await _context.Productlar
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Where(x => x.Id != id).Where(x => x.CategoryId == product.CategoryId).ToListAsync();
            if (product is null) return NotFound();

            DetailVM detailVM = new DetailVM
            {
                Products = product,
                RelatedProducts = relatedproducts,
            };

            return View(detailVM);
        }
    }
}
