using System.ComponentModel.DataAnnotations;
using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BakeryApp.Pages
{
    public class ContactsModel : PageModel
    {
        private readonly AppDbContext _db;
        public ContactsModel(AppDbContext db) { _db = db; }

        [BindProperty, Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Введите телефон")]
        public string Phone { get; set; } = "";
        [BindProperty, Required(ErrorMessage = "Введите сообщение")]
        public string Message { get; set; } = "";
        public bool Sent { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            _db.Feedbacks.Add(new Feedback { Name = Name, Phone = Phone, Message = Message });
            await _db.SaveChangesAsync();
            Sent = true;
            ModelState.Clear();
            Name = Phone = Message = "";
            return Page();
        }
    }
}