using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BakeryApp.Data;
using BakeryApp.Models;

namespace BakeryApp.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly AppDbContext _db;
        public CatalogModel(AppDbContext db) { _db = db; }

        public static readonly string[] Categories = { "Хлеб", "Выпечка", "Торты", "Напитки" };
        public List<Product> Products { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }

        public async Task OnGetAsync()
        {
            var q = _db.Products.AsQueryable();
            if (!string.IsNullOrEmpty(Category)) q = q.Where(p => p.Category == Category);
            Products = await q.OrderBy(p => p.Name).ToListAsync();
        }
    }
}