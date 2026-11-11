using System.Globalization;
using BakeryApp.Data;
using BakeryApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BakeryApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            var builder = WebApplication.CreateBuilder(args);

            // Taking conn string from settings
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Added database context
            builder.Services.AddDbContext<AppDbContext>(o => o.UseMySQL(connectionString));
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Configured Cookies
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/accessdenied";
                });
            // Add authorization
            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddRazorPages();

            builder.Services.Configure<RouteOptions>(o =>
            {
                o.LowercaseUrls = true;
                o.AppendTrailingSlash = false;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                DbSeeder.Seed(db, scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>());
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    if (ctx.Context.Request.Path.StartsWithSegments("/uploads"))
                    {
                        ctx.Context.Response.Headers.CacheControl = "public,max-age=86400";
                    }
                }
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages().WithStaticAssets();

            app.Run();
        }
    }
}