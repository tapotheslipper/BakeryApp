using System.ComponentModel.DataAnnotations;
using BakeryApp.Models;
using BakeryApp.Data;
using BakeryApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public class ReviewsModel : PageModel
    {
        private readonly AppDbContext _db;
        public ReviewsModel(AppDbContext db) { _db = db; }

        public List<Review> Items { get; set; } = new();
        [BindProperty, Range(1, 5)]
        public int Rating { get; set; } = 5;
        [BindProperty, Required(ErrorMessage = "Напишите отзыв"), StringLength(1000)]
        public string Text { get; set; } = "";
        public string? Message { get; set; }

        private async Task LoadAsync() => Items = await _db.Reviews.Where(r => r.IsPublished).OrderByDescending(r => r.Id).ToListAsync();

        public Task OnGetAsync() => LoadAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Identity?.IsAuthenticated != true)
                return RedirectToPage("/Login", new { returnUrl = "/reviews" });

            await LoadAsync();
            if (!ModelState.IsValid) return Page();

            var uid = User.UserId();
            var bought = await _db.Orders.AnyAsync(o => o.UserId == uid && o.Status == "Выполнен");
            if (!bought) { Message = "Отзыв можно оставить после выполенного заказа."; return Page(); }

            _db.Reviews.Add(new Review { UserId = uid, UserName = User.Identity!.Name ?? "", Rating = Rating, Text = Text });
            await _db.SaveChangesAsync();
            Message = "Спасибо! Отзыв появится после модерации.";
            ModelState.Clear(); Text = "";
            return Page();
        }
    }
}