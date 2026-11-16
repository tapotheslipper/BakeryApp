using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class ProductsModel : PageModel
    {
        private readonly AppDbContext _db;
        public ProductsModel(AppDbContext db) { _db = db; }

        public List<Product> Items { get; set; } = new();
        [BindProperty]
        public Product Input { get; set; } = new();

        public async Task OnGetAsync(int? editId)
        {
            Items = await _db.Products.OrderBy(p => p.Id).ToListAsync();
            if (editId != null) Input = await _db.Products.FindAsync(editId) ?? new Product();
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            ModelState.Remove("Input.Id");
            if (string.IsNullOrWhiteSpace(Input.Name)) { Items = await _db.Products.ToListAsync(); return Page(); }
            Input.Description ??= "";
            Input.Composition ??= "";
            Input.Calories ??= "";
            Input.Category ??= "";
            if (string.IsNullOrWhiteSpace(Input.ImageUrl)) Input.ImageUrl = "/images/default.svg";
            if (Input.Id == 0) _db.Products.Add(Input); else _db.Products.Update(Input);
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPOstDeleteAsync(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p != null) { _db.Products.Remove(p); await _db.SaveChangesAsync(); }
            return RedirectToPage();
        }
    }
}