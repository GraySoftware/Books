using Books.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BooksWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // url: /home/index would return the contents of this routine
        // View() serves up the index page by going to views, then the name of the controller
        // then getting the .cshtml file with the name of the action
        // so filepath views/home/index.cshtml
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}