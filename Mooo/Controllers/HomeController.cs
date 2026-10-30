using Microsoft.AspNetCore.Mvc;
using Mooo.Models;
using System.Diagnostics;

namespace Mooo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
