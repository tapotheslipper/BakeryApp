using BakeryApp.Data;
using BakeryApp.Models;
using BakeryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages.Admin
{
    [Authorize(Roles = "admin")]
    public class UsersModel : PageModel
    {
        private readonly AppDbContext _db;
        public UsersModel(AppDbContext db) { _db = db; }

        public static readonly string[] Roles = { "user", "manager", "admin" };
        public List<User> Items { get; set; } = new();

        public async Task OnGetAsync() => Items = await _db.Users.OrderBy(u => u.Id).ToListAsync();

        public async Task<IActionResult> OnPostAsync(int id, string role)
        {
            if (!Roles.Contains(role) || id == User.UserId()) return RedirectToPage();
            var u = await _db.Users.FindAsync(id);
            if (u != null) { u.Role = role; await _db.SaveChangesAsync(); }
            return RedirectToPage();
        }
    }
}