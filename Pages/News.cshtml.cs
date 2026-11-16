using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public class NewsModel : PageModel
    {
        private readonly AppDbContext _db;
        public NewsModel(AppDbContext db) { _db = db; }
        public List<News> Items { get; set; } = new();
        public async Task OnGetAsync() => Items = await _db.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
    }
}