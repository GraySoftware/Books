using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Books.DataAccess;
using Books.Models;
using Books.DataAccess.Repository.IRepository;

namespace BooksWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View(_UnitOfWork.Product.GetAll());
        }

        // GET: Product/Details/5
        public IActionResult Details(int? id)
        {
            IEnumerable<Product> objProductList = _UnitOfWork.Product.GetAll();
            if (id == null || objProductList == null)
            {
                return NotFound();
            }

            var Product = objProductList.FirstOrDefault(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Product Product)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Product.Add(Product);
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _UnitOfWork.Product.GetAll == null)
            {
                return NotFound();
            }

            var Product = _UnitOfWork.Product.GetFirstOrDefault(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            return View(Product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Product Product)
        {
            if (id != Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _UnitOfWork.Product.Update(Product);
                    _UnitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(Product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Product);
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _UnitOfWork.Product.GetAll == null)
            {
                return NotFound();
            }

            var Product = _UnitOfWork.Product.GetFirstOrDefault(m => m.Id == id);
            if (Product == null)
            {
                return NotFound();
            }

            return View(Product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_UnitOfWork.Product.GetAll == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var Product = _UnitOfWork.Product.GetFirstOrDefault(m => m.Id == id);
            if (Product != null)
            {
                _UnitOfWork.Product.Remove(Product);
            }

            _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            if (_UnitOfWork.Product.GetFirstOrDefault(e => e.Id == id) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

