using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BakeryApp.Data;
using BakeryApp.Models;

namespace BakeryApp.Pages
{
    public class CatalogModel : PageModel
    {
        private readonly AppDbContext _context;
        public CatalogModel(AppDbContext context) { _context = context; }
        public List<Product> Products { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(p => p.Category == Category);
            }
            Products = await query.ToListAsync();
        }
    }
}