using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Areas.MultiShopAdmin.ViewModels;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.Utilities.Enums;
using MultiShop.Utilities.Extensions;

namespace MultiShop.Areas.MultiShopAdmin.Controllers
{
    [Area("MultiShopAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Productlar> productlars= await _context.Productlar               
                .Include(p=>p.Category)
                .Include(p=>p.ProductImages.Where(p=>p.IsPrimary==true))
                .ToListAsync();
            return View(productlars);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                return View(productVM);
            }
            bool result2 = await _context.Categories.AnyAsync(x => x.Id == productVM.CategoryId);
            if (!result2)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                ModelState.AddModelError("CategoryId", "We have not so Category with this Id");
                return View(productVM);
            }
           
            foreach (var colorId in productVM.ColorIds)
            {
                bool ColorResult = await _context.Colors.AnyAsync(x => x.Id == colorId);
                if (!ColorResult)
                {
                    ModelState.AddModelError("ColorIds", "We have not so Color with this Id");
                    return View();
                }
            } 

            //---------------MAIN PHOTO CHECKING------------------
            if (!productVM.MainPhoto.ValidateFileType(FileHelper.Image))
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                ModelState.AddModelError("MainPhoto", "File Type is not Matching");
                return View(productVM);
            }
            if (!productVM.MainPhoto.ValidateSize(SizeHelper.gb))
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                ModelState.AddModelError("MainPhoto", "File Size is not Suitable");
                return View(productVM);
            }
            //---------------HOVER PHOTO CHECKING------------------
            if (!productVM.HoverPhoto.ValidateFileType(FileHelper.Image))
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                ModelState.AddModelError("HoverPhoto", "File Type is not Matching");
                return View(productVM);
            }
            if (!productVM.HoverPhoto.ValidateSize(SizeHelper.gb))
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                ModelState.AddModelError("HoverPhoto", "File Size is not Suitable");
                return View(productVM);
            }

            ProductImage main = new ProductImage
            {
                IsPrimary = true,
                Url = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img"),

            };

            ProductImage hover = new ProductImage
            {
                IsPrimary = false,
                Url = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img"),
            };


            Productlar product = new Productlar
            {
                Name = productVM.Name,
                Description = productVM.Description,
                Price = productVM.Price,
                CategoryId = (int)productVM.CategoryId,
                ProductColors = new List<ProductColor>(),
                ProductImages = new List<ProductImage> { main, hover }
            };
            TempData["Message"] = "";
            foreach (IFormFile photo in productVM.Photos ?? new List<IFormFile>())
            {
                if (!photo.ValidateFileType(FileHelper.Image))
                {
                    TempData["Message"] += $"<div class=\"alert alert-danger\" role=\"alert\"> {photo.FileName} file's Type is not suitable,That's why creating file's Mission Failed </div>";
                    continue;
                }

                if (!photo.ValidateSize(SizeHelper.gb))
                {
                    TempData["Message"] += $"<div class=\"alert alert-danger\" role=\"alert\"> {photo.FileName} file's Size is not suitable,That's why creating file's Mission Failed </div>";
                    continue;
                }
                product.ProductImages.Add(new ProductImage
                {
                    IsPrimary = null,
                    Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "img")
                });
            }

            foreach (int colorId in productVM.ColorIds)
            {
                ProductColor productColor = new ProductColor
                {
                    ColorId = colorId,
                };
                product.ProductColors.Add(productColor);
            }
            
            await _context.Productlar.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int id)
        {
            Productlar product = await _context.Productlar
                .Include(x => x.Category)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Productlar product = await _context.Productlar
                .Include(x => x.ProductColors)
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            UpdateProductVM productVM = new UpdateProductVM
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ColorIds = product.ProductColors.Select(x => x.ColorId).ToList(),
                Categories = await _context.Categories.ToListAsync(),
                Colors = await _context.Colors.ToListAsync(),
                ProductImages = product.ProductImages,
            };
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductVM productVM)
        {
            Productlar existed = await _context.Productlar
                .Include(x => x.ProductColors)
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(y => y.Id == id);
            if (!ModelState.IsValid)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                productVM.ProductImages = existed.ProductImages;
                return View(productVM);
            }

            if (existed is null)
            {
                return NotFound();
            }
            if (productVM.MainPhoto is not null)
            {
                if (!productVM.MainPhoto.ValidateFileType(FileHelper.Image))
                {
                    productVM.Categories = await _context.Categories.ToListAsync();
                    productVM.Colors = await _context.Colors.ToListAsync();
                    productVM.ProductImages = existed.ProductImages;
                    ModelState.AddModelError("MainPhoto", "File Type is not Matching");
                    return View(productVM);
                }
                if (!productVM.MainPhoto.ValidateSize(SizeHelper.gb))
                {
                    productVM.Categories = await _context.Categories.ToListAsync();
                    productVM.Colors = await _context.Colors.ToListAsync();
                    productVM.ProductImages = existed.ProductImages;
                    ModelState.AddModelError("MainPhoto", "File Size is not Suitable");
                    return View(productVM);
                }
            }
            if (productVM.HoverPhoto is not null)
            {
                if (!productVM.HoverPhoto.ValidateFileType(FileHelper.Image))
                {
                    productVM.Categories = await _context.Categories.ToListAsync();
                    productVM.Colors = await _context.Colors.ToListAsync();
                    productVM.ProductImages = existed.ProductImages;
                    ModelState.AddModelError("HoverPhoto", "File Type is not Matching");
                    return View(productVM);
                }
                if (!productVM.HoverPhoto.ValidateSize(SizeHelper.gb))
                {
                    productVM.Categories = await _context.Categories.ToListAsync();
                    productVM.Colors = await _context.Colors.ToListAsync();
                    productVM.ProductImages = existed.ProductImages;
                    ModelState.AddModelError("HoverPhoto", "File Size is not Suitable");
                    return View(productVM);
                }
            }
            bool result = await _context.Categories.AnyAsync(x => x.Id == productVM.CategoryId);
            if (!result)
            {
                productVM.Categories = await _context.Categories.ToListAsync();
                productVM.Colors = await _context.Colors.ToListAsync();
                productVM.ProductImages = existed.ProductImages;
                ModelState.AddModelError("CategoryId", "We have not so Category");
                return View();
            }
            
            

            if (productVM.MainPhoto is not null)
            {
                string filename = await productVM.MainPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");
                ProductImage existedImg = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == true);
                existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ProductImages.Remove(existedImg);
                existed.ProductImages.Add(new ProductImage
                {
                    IsPrimary = true,
                    Url = filename
                });
            }
            if (productVM.HoverPhoto is not null)
            {
                string filename = await productVM.HoverPhoto.CreateFileAsync(_env.WebRootPath, "assets", "img");
                ProductImage existedImg = existed.ProductImages.FirstOrDefault(pi => pi.IsPrimary == false);
                existedImg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ProductImages.Remove(existedImg);
                existed.ProductImages.Add(new ProductImage
                {
                    IsPrimary = false,
                    Url = filename
                });
            }



            //--------------------Color
            foreach (var pColor in existed.ProductColors)
            {
                if (!productVM.ColorIds.Exists(cId => cId == pColor.ColorId))
                {
                    _context.ProductColor.Remove(pColor);
                }
            }
            foreach (int cId in productVM.ColorIds)
            {
                if (!existed.ProductColors.Any(pc => pc.ColorId == cId))
                {
                    existed.ProductColors.Add(new ProductColor
                    {
                        ColorId = cId
                    });
                }
            }
            //-------------------Size
           
            if (productVM.ImageIds is null)
            {
                productVM.ImageIds = new List<int>();
            }
            List<ProductImage> removeable = existed.ProductImages.Where(pi => !productVM.ImageIds.Exists(imgId => imgId == pi.Id) && pi.IsPrimary == null).ToList();
            foreach (ProductImage reimg in removeable)
            {
                reimg.Url.DeleteFile(_env.WebRootPath, "assets", "img");
                existed.ProductImages.Remove(reimg);

            }
            TempData["Message"] = "";
            foreach (IFormFile photo in productVM.Photos ?? new List<IFormFile>())
            {
                if (!photo.ValidateFileType(FileHelper.Image))
                {
                    TempData["Message"] += $"<div class=\"alert alert-danger\" role=\"alert\"> {photo.FileName} file's Type is not suitable,That's why creating file's Mission Failed </div>";
                    continue;
                }

                if (!photo.ValidateSize(SizeHelper.gb))
                {
                    TempData["Message"] += $"<div class=\"alert alert-danger\" role=\"alert\"> {photo.FileName} file's Size is not suitable,That's why creating file's Mission Failed </div>";
                    continue;
                }
                existed.ProductImages.Add(new ProductImage
                {
                    IsPrimary = null,
                    Url = await photo.CreateFileAsync(_env.WebRootPath, "assets", "img")
                });
            }


            existed.Name = productVM.Name;
            existed.Price = productVM.Price;
            existed.Description = productVM.Description;
            existed.CategoryId = productVM.CategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Productlar product = await _context.Productlar.Include(x => x.ProductImages).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) return NotFound();
            foreach (var image in product.ProductImages ?? new List<ProductImage>())
            {
                image.Url.DeleteFile(_env.WebRootPath, "assets", "img");
            }
            _context.Productlar.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

