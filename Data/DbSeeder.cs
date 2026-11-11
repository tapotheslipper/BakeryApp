using Microsoft.AspNetCore.Identity;
using BakeryApp.Models;

namespace BakeryApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext db, IPasswordHasher<User> hasher)
        {
            if (!db.Users.Any())
            {
                User admin = new User { Email = "admin@example.com", FullName = "Admin1", Role = "admin" };
                admin.PasswordHash = hasher.HashPassword(admin, "password");
                User mgr = new User { Email = "manager@example.com", FullName = "Manager1", Role = "manager" };
                mgr. PasswordHash = hasher.HashPassword(mgr, "password");
                db.Users.AddRange(admin, mgr);
            }
            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Name = "Хлеб 1", Category = "Хлеб", Price = 90, IsRecommended = true, Description = "Ржаной хлеб.", Calories = "208 ккал / 100 г", Composition = "Ржаная мука, соль" },
                    new Product { Name = "Багет 1", Category = "Хлеб", Price = 75, Description = "Французский багет, наверное.", Calories = "262 ккал / 100 г", Composition = "Мука пшено, вода, дрожжи, соль" },
                    new Product { Name = "Круассан", Category = "Выпечка", Price = 110, IsRecommended = true, Description = "На масле.", Calories = "406 ккал / 100 г", Composition = "Мука, масло сливочное, молоко" },
                    new Product { Name = "Булочка с корицей", Category = "Выпечка", Price = 95, IsRecommended = true, Description = "Тёплая, с глазурью.", Calories = "330 ккал / 90 г", Composition = "Мука, корица, сахар, масло" },
                    new Product { Name = "Тест 2", Category = "Выпечка", Price = 42, Description = "Тестирование", Calories = "", Composition = "" },
                    new Product { Name = "Пирожок с яблоком", Category = "Выпечка", IsRecommended = true, Description = "Тестирование 3", Calories = "///", Composition = "---" },
                    new Product { Name = "Торт Наполеон", Category = "Торты", Price = 1200, IsRecommended = true, Description = "Торты везде.", Calories = "9384844", Composition = "sdfggdsf" },
                    new Product { Name = "Чик", Category = "Торты", Price = 1100, Description = "sdfgds------------ddddddddddddddddddddd", Calories = "321 123", Composition = "123, 234, 345" }
                );
            }
            if (!db.Promotions.Any())
            {
                db.Promotions.AddRange(
                    new Promotion { Title = "Утренний набор", Discount = "20%", Text = "Два и два до 10:00 со скидкой." },
                    new Promotion { Title = "Хлебный мякиш", Discount = "42%", Text = "Только вечером от 09:00." }
                );
            }
            if (!db.News.Any())
            {
                db.News.AddRange(
                    new News { Title = "Новая выпечка!", Text = "В меню есть яблоки теперь." },
                    new News { Title = "Урок", Text = "Вас научат оставлять больше денег." },
                    new News { Title = "Мы расширили зал!", Text = "Это был рейдерский захват соседей." }
                );
            }
            if (!db.Reviews.Any())
            {
                db.Reviews.AddRange(
                    new Review { UserName = "Анна", Rating = 5, Text = "Лучшие на районе.", IsPublished = true },
                    new Review { UserName = "Толстый", Rating = 1, Text = "Мало жира.", IsPublished = false }
                );
            }
            db.SaveChanges();
        }
    }
}