using Microsoft.AspNetCore.Mvc;

namespace Mooo.Controllers
{
    public class AnimalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
