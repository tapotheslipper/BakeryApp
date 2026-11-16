using System.ComponentModel.DataAnnotations;
using BakeryApp.Models;
using BakeryApp.Data;
using BakeryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly AppDbContext _db;
        public ProfileModel(AppDbContext db) { _db = db; }

        [BindProperty, Required(ErrorMessage = "Введите ФИО")]
        public string FullName { get; set; } = "";
        [BindProperty]
        public string? Phone { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool Ordered { get; set; }
        public string Email { get; set; } = "";
        public List<Order> Orders { get; set; } = new();

        private async Task LoadAsync()
        {
            var uid = User.UserId();
            var u = await _db.Users.FindAsync(uid);
            Email = u!.Email;
            Orders = await _db.Orders.Where(o => o.UserId == uid).OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task OnGetAsync()
        {
            await LoadAsync();
            var u = await _db.Users.FindAsync(User.UserId());
            FullName = u!.FullName; Phone = u.Phone;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadAsync();
            if (!ModelState.IsValid) return Page();
            var u = await _db.Users.FindAsync(User.UserId());
            u!.FullName = FullName; u.Phone = Phone ?? "";
            await _db.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}