using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages.Admin
{
    [Authorize(Roles = "admin,manager")]
    public class ReviewsModel : PageModel
    {
        private readonly AppDbContext _db;
        public ReviewsModel(AppDbContext db) { _db = db; }
        public List<Review> Items { get; set; } = new();

        public async Task OnGetAsync() => Items = await _db.Reviews.OrderBy(r => r.IsPublished).ThenByDescending(r => r.Id).ToListAsync();

        public async Task<IActionResult> OnPostAsync(int id, bool publish)
        {
            if (!User.IsInRole("admin")) return Forbid();
            var r = await _db.Reviews.FindAsync(id);
            if (r != null) { r.IsPublished = publish; await _db.SaveChangesAsync(); }
            return RedirectToPage();
        }
    }
}