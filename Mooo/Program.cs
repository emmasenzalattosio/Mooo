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
                (opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("OnlyFarms")));

            var app = builder.Build();

    

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
