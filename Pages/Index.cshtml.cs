using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }

        public List<Product> Featured { get; set; } = new();
        public List<Promotion> Promos { get; set; } = new();
        public List<News> LatestNews { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();

        public async Task OnGetAsync()
        {
            Featured = await _db.Products.Where(p => p.IsRecommended).Take(4).ToListAsync();
            Promos = await _db.Promotions.Take(2).ToListAsync();
            LatestNews = await _db.News.OrderByDescending(n => n.CreatedAt).Take(3).ToListAsync();
            Reviews = await _db.Reviews.Where(r => r.IsPublished).OrderByDescending(r => r.Id).Take(3).ToListAsync();
        }
    }
}
