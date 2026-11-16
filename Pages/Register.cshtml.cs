using System.ComponentModel.DataAnnotations;
using BakeryApp.Models;
using BakeryApp.Data;
using BakeryApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        public RegisterModel(AppDbContext db, IPasswordHasher<User> hasher) { _db = db; _hasher = hasher; }

        [BindProperty, Required(ErrorMessage = "Введите ФИО")]
        public string FullName { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Введите email"), EmailAddress(ErrorMessage = "Неверный email")]
        public string Email { get; set; } = "";
        [BindProperty]
        public string? Phone { get; set; }
        [BindProperty, Required(ErrorMessage = "Введите пароль"), MinLength(6, ErrorMessage = "Минимум 6 символов")]
        public string Password { get; set; } = "";

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var email = Email.Trim().ToLower();
            if (await _db.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError(nameof(Email), "Такой email уже занят");
                return Page();
            }

            var user = new User { Email = email, FullName = FullName, Phone = Phone ?? "", Role = "user" };
            user.PasswordHash = _hasher.HashPassword(user, Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            await AuthHelper.SignInAsync(HttpContext, user);
            return RedirectToPage("/Profile");
        }
    }
}