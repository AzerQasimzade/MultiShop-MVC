using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiShop.DAL;
using MultiShop.Models;

namespace MultiShop.Services
{
    public class LayoutService
    {
        private readonly AppDbContext _context;

        public LayoutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Dictionary<string,string>> GetSettingsAsync()
        {
            Dictionary<string, string> settings=await _context.Settings.ToDictionaryAsync(s=>s.Key,s=>s.Value);
            return settings;
        }
        public List<string> GetCategoryNames()
        {
            List<Category> categories = _context.Categories.ToList();
            List<string> categoryNames = categories.Select(c => c.Name).ToList();
            return categoryNames;
        }
    }
}
