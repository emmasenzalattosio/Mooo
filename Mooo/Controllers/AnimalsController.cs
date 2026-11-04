using Microsoft.AspNetCore.Mvc;
using Mooo.Models;

namespace Mooo.Controllers
{
    public class AnimalsController : Controller
    {
        private readonly OnlyFarmsDbContext DB;

        // Constructor: connects our controller to the SQL Server database
        public AnimalsController(OnlyFarmsDbContext db) => DB = db;
        

        // 1. READ: Shows all animals from the database
        public IActionResult Index(string username)
        {
            ViewBag.Username = username;

            List<Animals> tiere = DB.Animal.OrderByDescending(a => a.Price).ToList();
            return View(tiere);
        }



        // ZSK-RECHNER: Euro -> Tiere umrechnen
        [HttpPost]
        public IActionResult EuroToAnimals(int euroBudget)
        {
            List<Animals> tiere = DB.Animal.OrderByDescending(a => a.Price).ToList();

            int restEuro = euroBudget;
            List<string> ergebnisListe = new List<string>();

            foreach (var tier in tiere)
            {
                if (tier.Price > 0)
                {
                    int anzahl = restEuro / tier.Price;
                    if (anzahl > 0)
                    {
                        ergebnisListe.Add(anzahl + "x " + tier.Name + " (je " + tier.Price + " €)");
                        restEuro = restEuro % tier.Price;
                    }
                }
            }

            ViewBag.EuroBudget = euroBudget;
            ViewBag.EuroToAnimalsList = ergebnisListe;
            ViewBag.RestEuro = restEuro;

            return View("Index", tiere);
        }


        // ZSK-RECHNER: Tiere -> Euro umrechnen
        [HttpPost]
        public IActionResult AnimalsToEuro(int animalId, int anzahl)
        {
            List<Animals> tiere = DB.Animal.OrderByDescending(a => a.Price).ToList();

            Animals tier = DB.Animal.Find(animalId)!;
            if (tier != null)
            {
                ViewBag.TotalEuroResult = tier.Price * anzahl;
                ViewBag.SelectedAnimalText = anzahl + "x " + tier.Name;
            }

            return View("Index", tiere);
        }

        // ZSK-RECHNER (Kann-Kriterium): Tiere direkt tauschen
        [HttpPost]
        public IActionResult TradeAnimals(int fromAnimalId, int fromCount, int toAnimalId)
        {
            List<Animals> tiere = DB.Animal.OrderByDescending(a => a.Price).ToList();

            Animals quellTier = DB.Animal.Find(fromAnimalId)!;
            Animals zielTier = DB.Animal.Find(toAnimalId)!;

            if (quellTier != null && zielTier != null && zielTier.Price > 0)
            {
                int tauschWertInEuro = quellTier.Price * fromCount;
                int bekommeneTiere = tauschWertInEuro / zielTier.Price;
                int restWert = tauschWertInEuro % zielTier.Price;

                ViewBag.TradeSummary = "Für " + fromCount + "x " + quellTier.Name + " (" + tauschWertInEuro + " €) bekommst du:";
                ViewBag.TradeMainResult = bekommeneTiere + "x " + zielTier.Name;
                ViewBag.TradeRestEuro = restWert;
            }

            return View("Index", tiere);
        }

        // CREATE / EDIT PAGE: Opens the Creator form
        [HttpGet]
        public IActionResult Creator(int? id)
        {
            ViewBag.AllAnimals = DB.Animal.OrderByDescending(a => a.Price).ToList();

            if (id != null)
            {
                Animals gefundenesTier = DB.Animal.Find(id)!;
                if (gefundenesTier != null)
                {
                    return View(gefundenesTier);
                }
            }

            // Default values when creating a new animal
            Animals neuesTier = new Animals();
            neuesTier.Name = "Berta die Kuh";
            neuesTier.Category = "Kuh";
            neuesTier.Price = 2800;
            neuesTier.Experience = "Newcomer";
            neuesTier.Provenience = "cow_lick(real).jpg";

            return View(neuesTier);
        }

        // SAVE TO DATABASE: Adds a new animal OR updates an existing animal
        [HttpPost]
        public IActionResult SaveAnimal(Animals formTier)
        {
            // Make sure no text field is null (SQL Server does not allow NULL strings)
            if (formTier.Name == null) { formTier.Name = "Neues Tier"; }
            if (formTier.Category == null) { formTier.Category = "Kuh"; }
            if (formTier.Description == null) { formTier.Description = "Frisch auf der Weide!"; }
            if (formTier.Experience == null) { formTier.Experience = "OnlyFarms Star"; }
            if (formTier.Provenience == null) { formTier.Provenience = "licking_cow.gif"; }

            if (formTier.ID == 0)
            {
                DB.Animal.Add(formTier);
            }
            else
            {
                Animals dbTier = DB.Animal.Find(formTier.ID)!;
                if (dbTier != null)
                {
                    dbTier.Name = formTier.Name;
                    dbTier.Category = formTier.Category;
                    dbTier.Price = formTier.Price;
                    dbTier.Description = formTier.Description;
                    dbTier.Experience = formTier.Experience;
                    dbTier.Provenience = formTier.Provenience;
                }
            }

            DB.SaveChanges();
            return RedirectToAction("Index");
        }

        // DELETE: Removes an animal from the database
        public IActionResult Delete(int id)
        {
            Animals tier = DB.Animal.Find(id)!;
            if (tier != null)
            {
                DB.Animal.Remove(tier);
                DB.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
