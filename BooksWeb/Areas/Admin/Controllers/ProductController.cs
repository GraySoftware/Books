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
using Books.Models.ViewModels;

namespace BooksWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View();
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

        // GET: Product/Edit/5
        public IActionResult Upsert(int? id)
        {
            //Product product = new();
            //// here we are creating dropdown lists populated with
            //// information from the Category table is passed using ViewBag
            //// information from the CoverType table is passed using ViewData
            //IEnumerable<SelectListItem> CategoryList = _UnitOfWork.Category.GetAll().Select(
            //    u=> new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });
            //IEnumerable<SelectListItem> CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(
            //    u => new SelectListItem
            //    {
            //        Text = u.Name,
            //        Value = u.Id.ToString()
            //    });

            // instead of doing it the way we were above
            // here we are leveraging the ProductVM (Product View Model) we defined in DataAccess.ViewModels
            // the advantage of doing it this way is now the view is tightly bound to the productVM rather\
            // than loosley bound to multiple models
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _UnitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _UnitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                // create product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            {
                //update product
            }

            return View();
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                _UnitOfWork.Product.Add(obj.Product);
                _UnitOfWork.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
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

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _UnitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }
        #endregion
    }
}

