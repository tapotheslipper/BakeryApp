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
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;
        public LoginModel(AppDbContext db, IPasswordHasher<User> hasher) { _db = db; _hasher = hasher; }

        [BindProperty, Required(ErrorMessage = "Введите email")]
        public string Email { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; } = "";
        [BindProperty(SupportsGet = true)]
        public string? ReturnUrl { get; set; }
        public string? Error { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var email = Email.Trim().ToLower();
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || _hasher.VerifyHashedPassword(user, user.PasswordHash, Password) == PasswordVerificationResult.Failed)
            {
                Error = "Неверный email или пароль"; return Page();
            }

            await AuthHelper.SignInAsync(HttpContext, user);
            return LocalRedirect(Url.IsLocalUrl(ReturnUrl) ? ReturnUrl! : "/");
        }
    }
}