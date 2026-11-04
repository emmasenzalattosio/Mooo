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

                // If running on Railway/new server where the database is empty, seed your animals
                if (!db.Animal.Any())
                {
                    db.Animal.AddRange(
                        new Animals
                        {
                            Name = "Kuh „Big-Euter Berta“ (VIP)",
                            Category = "Kuh",
                            Price = 2800,
                            Description = "Zeigt für Premium-Abonnenten unzensiert alle vier Zitzen beim morgendlichen Melken. 100% Natur, kein Silikon im Euter!",
                            Experience = "VIP",
                            Provenience = "licking_cow.gif"
                        },
                        new Animals
                        {
                            Name = "Schaf „Nackte Chantal“",
                            Category = "Schaf",
                            Price = 650,
                            Description = "Frisch geschoren und komplett hüllenlos! Liebt es, wenn man ihr tief ins flauschige Vlies greift.",
                            Experience = "Top 1% Weide",
                            Provenience = "sheep_licking.gif"
                        },
                        new Animals
                        {
                            Name = "Ziege „Horny Helga“",
                            Category = "Ziege",
                            Price = 500,
                            Description = "Extrem lange Hörner, freche Zunge und knabbert an allem, was man ihr hinhält. Springt für Geld auf jeden Tisch.",
                            Experience = "Kletter-Star",
                            Provenience = "giphy-2753670406.gif"
                        },
                        new Animals
                        {
                            Name = "Kleine Ziege „Dirty Lilli“",
                            Category = "Kleine Ziege",
                            Price = 50,
                            Description = "Noch neu im Heu, aber weiß schon genau, wie man für eine Handvoll Kraftfutter vor der Kamera wackelt.",
                            Experience = "Newcomer",
                            Provenience = "freet.jpg"
                        }
                    );
                    db.SaveChanges();
                }
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
