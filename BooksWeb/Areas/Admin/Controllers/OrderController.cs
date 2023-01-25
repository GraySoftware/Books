using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
