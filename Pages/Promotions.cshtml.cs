using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public class PromotionsModel : PageModel
    {
        private readonly AppDbContext _db;
        public PromotionsModel(AppDbContext db) { _db = db; }
        public List<Promotion> Items { get; set; } = new();
        public async Task OnGetAsync() => Items = await _db.Promotions.ToListAsync();
    }
}