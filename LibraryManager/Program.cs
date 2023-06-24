using LibraryManager.Models;
using LibraryManager.Models.Repository;
using LibraryManager.Models.Repository.Contracts;
using System.Globalization;

namespace LibraryManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSqlite<Contexts>("");
            builder.Services.AddTransient<IBookRepository, BookRepository>();
            builder.Services.AddTransient<IMemberRepository, MemberRepository>();

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Library/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Library}/{action=Index}/{id?}");

            app.Run();
        }
    }
}