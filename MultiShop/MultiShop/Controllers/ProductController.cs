using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.ViewModels;

namespace MultiShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        //public async Task<IActionResult> Shop(int order)
        //{
        //    List<Productlar>productlars =await _context.Productlar.ToListAsync();
        //    switch (order)
        //    {
        //        case 1:
        //            productlars = productlars.OrderBy(p => p.Name).ToList();
        //            break;
        //        case 2:
        //            productlars = productlars.OrderBy(p=>p.Price).ToList();
        //            break;  
        //        case 3:
        //            productlars = productlars.OrderByDescending(p => p.Id).ToList();  
        //            break;
        //    }
        //    HomeVM homeVM = new HomeVM
        //    {
        //        Categories = await _context.Categories.Include(c => c.Productlar).ToListAsync(),
        //        Productlar = await _context.Productlar.ToListAsync(),
        //        Order = order
        //    };
        //    return View(homeVM);
        //}
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
        public async Task<IActionResult> Details()
        {
            //Product product = await _context.Products
            //    .Include(x => x.Category)
            //    .Include(x => x.ProductImages)
            //    .Include(x => x.ProductTags)
            //    .ThenInclude(pt => pt.Tag)
            //    .Include(x => x.ProductColors)
            //    .ThenInclude(x => x.Color)
            //    .Include(x => x.ProductSizes)
            //    .ThenInclude(x => x.Size)
            //    .FirstOrDefaultAsync(x => x.Id == id);
            //if (product is null)
            //{
            //    return NotFound();
            //}
            return View();
        }
    }
}
