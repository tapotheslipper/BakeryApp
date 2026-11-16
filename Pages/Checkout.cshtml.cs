using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using BakeryApp.Models;
using BakeryApp.Data;
using BakeryApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public record CartLine(int Id, int Qty);

    [Authorize]
    public class CheckoutModel : PageModel
    {
        private readonly AppDbContext _db;
        public CheckoutModel(AppDbContext db) { _db = db; }

        [BindProperty, Required(ErrorMessage = "Введите ФИО")]
        public string FullName { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Введите телефон")]
        public string Phone { get; set; } = "";
        [BindProperty]
        public string DeliveryType { get; set; } = "Самовывоз";
        [BindProperty]
        public string? Address { get; set; }
        [BindProperty]
        public string? CartJson { get; set; }
        public string? Error { get; set; }

        public async Task OnGetAsync()
        {
            var u = await _db.Users.FindAsync(User.UserId());
            if (u != null) { FullName = u.FullName; Phone = u.Phone; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            if (DeliveryType == "Доставка" && string.IsNullOrWhiteSpace(Address))
            {
                Error = "Укажите адрес доставки";
                return Page();
            }

            List<CartLine> lines;
            try
            {
                lines = JsonSerializer.Deserialize<List<CartLine>>(CartJson ?? "[]", new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            }
            catch { lines = new(); }
            lines = lines.Where(l => l.Qty > 0).ToList();

            var ids = lines.Select(l => l.Id).ToList();
            var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToDictionaryAsync(p => p.Id);
            var order = new Order
            {
                UserId = User.UserId(), FullName = FullName, Phone = Phone,
                DeliveryType = DeliveryType, Address = Address ?? ""
            };
            foreach (var l in lines)
            {
                if (!products.TryGetValue(l.Id, out var p)) continue;
                order.OrderItems.Add(new OrderItem { ProductId = p.Id, Count = l.Qty, Price = p.Price });
                order.TotalSum += p.Price * l.Qty;
            }
            if (!order.OrderItems.Any()) { Error = "Корзина пуста"; return Page(); }

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            return RedirectToPage("/Profile", new { ordered = true });
        }
    }
}