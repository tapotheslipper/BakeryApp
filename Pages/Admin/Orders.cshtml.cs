using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages.Admin
{
    [Authorize(Roles = "admin,manager")]
    public class OrdersModel : PageModel
    {
        private readonly AppDbContext _db;
        public OrdersModel(AppDbContext db) { _db = db; }

        public static readonly string[] Statuses = { "Новый", "В работе", "Готов", "Выполнен", "Отменен" };
        public List<Order> Orders { get; set; } = new();

        public async Task OnGetAsync() => Orders = await _db.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Product).OrderByDescending(c => c.CreatedAt).ToListAsync();

        public async Task<IActionResult> OnPostAsync(int id, string status)
        {
            if (!Statuses.Contains(status)) return BadRequest();
            var o = await _db.Orders.FindAsync(id);
            if (o != null) { o.Status = status; await _db.SaveChangesAsync(); }
            return RedirectToPage();
        }
    }
}