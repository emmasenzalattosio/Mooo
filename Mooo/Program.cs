using Microsoft.EntityFrameworkCore;
using Mooo.Models;
namespace Mooo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<OnlyFarmsDbContext>
                (opts => opts.UseSqlite(builder.Configuration.GetConnectionString("OnlyFarms")));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<OnlyFarmsDbContext>();
                db.Database.EnsureCreated();
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.UseStaticFiles();
            app.UseRouting();
            app.Run();
        }
    }
}
