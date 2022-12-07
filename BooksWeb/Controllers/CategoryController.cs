using BooksWeb.Data; // gives us access to applicationDbContext
using BooksWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        // the ApplicationDbContext db i nthe arguments gives us access to the applicationDbContext.cs class 
        // through dependency injection that is made available in the program.cs file.
        // thus this object will be configured the same way (using the same connection string)
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            // this accesses the database, retrieves the entire categories table and returns it as a list
            IEnumerable<Category> objCategoryList = _db.Categories;
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        // Post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
