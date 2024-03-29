﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.Areas.MultiShopAdmin.ViewModels;
using MultiShop.DAL;
using MultiShop.Models;
using MultiShop.Utilities.Enums;
using MultiShop.Utilities.Extensions;


namespace MultiShop.Areas.MultiShopAdmin.Controllers
{
    [Area("MultiShopAdmin")]
    public class SlideController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SlideController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();
            return View(slides);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSlideVM slideVM)
        { 
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!slideVM.Photo.ValidateFileType(FileHelper.Image))
            {
                ModelState.AddModelError("Photo", "File tipi uygun deyil");
                return View();
            }
            if (!slideVM.Photo.ValidateSize(SizeHelper.mb))
            {
                ModelState.AddModelError("Photo", "File olcusu 1 mb den boyuk olmamalidir");
                return View();
            }
            string filename = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
            Slide slide = new Slide
            {
                Image = filename,
                Title = slideVM.Title,
                Description = slideVM.Description,
                Order = slideVM.Order
            };
            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            Slide existed = await _context.Slides.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            UpdateSlideVM slideVM = new UpdateSlideVM
            {
                Description = existed.Description,
                Order = existed.Order,
                Title = existed.Title,
                Image = existed.Image
            };
            return View(slideVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSlideVM slideVM)
        {      
            if (!ModelState.IsValid)
            {
                return View(slideVM);
            }
            Slide existed = await _context.Slides.FirstOrDefaultAsync(y => y.Id == id);

            if (existed is null)
            {
                return NotFound();
            }
            if (slideVM.Photo is not null)
            {
                if (!slideVM.Photo.ContentType.Contains("image/"))
                {
                    ModelState.AddModelError("Photo", "Shekilin file type-i Image olmalidir");
                    return View(slideVM);
                }
                if (slideVM.Photo.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("Photo", "File olcusu 2mb dan cox olmamalidir");
                    return View(slideVM);
                }
                string filename = await slideVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
                existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");

                existed.Image = filename;
            }
            existed.Title = slideVM.Title;
            existed.Description = slideVM.Description;
            existed.Order = slideVM.Order;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id<=0)
            {
                return BadRequest();
            }
            Slide existed= await _context.Slides.FirstOrDefaultAsync(x => x.Id == id);
            if (existed is null)
            {
                return NotFound();
            }
            string path = Path.Combine(_env.WebRootPath, "assets/img", existed.Image);


            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _context.Slides.Remove(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
