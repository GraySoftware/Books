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
using Microsoft.AspNetCore.Authorization;
using Books.Utility;

namespace BooksWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // GET: Company
        public IActionResult Index()
        {
            return View();
        }

        // GET: Company/Details/5
        public IActionResult Details(int? id)
        {
            IEnumerable<Company> objCompanyList = _UnitOfWork.Company.GetAll();
            if (id == null || objCompanyList == null)
            {
                return NotFound();
            }

            var Company = objCompanyList.FirstOrDefault(m => m.Id == id);
            if (Company == null)
            {
                return NotFound();
            }

            return View(Company);
        }

        // GET: Company/Edit/5
        public IActionResult Upsert(int? id)
        {
            Company company = new();

            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }

            return View();
        }

        // POST: Company/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {

            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _UnitOfWork.Company.Add(company);
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _UnitOfWork.Company.Update(company);
                    TempData["success"] = "Company updated successfully";
                }
                _UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //// GET: Company/Delete/5
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || _UnitOfWork.Company.GetAll == null)
        //    {
        //        return NotFound();
        //    }

        //    var Company = _UnitOfWork.Company.GetFirstOrDefault(m => m.Id == id);
        //    if (Company == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(Company);
        //}



        private bool CompanyExists(int id)
        {
            if (_UnitOfWork.Company.GetFirstOrDefault(e => e.Id == id) == null)
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
            var CompanyList = _UnitOfWork.Company.GetAll();
            return Json(new { data = CompanyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _UnitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _UnitOfWork.Company.Remove(obj);
            _UnitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}

