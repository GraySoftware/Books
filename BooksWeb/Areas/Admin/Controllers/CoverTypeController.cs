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
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        // GET: CoverType
        public async Task<IActionResult> Index()
        {
            return View(_UnitOfWork.CoverType.GetAll());
        }

        // GET: CoverType/Details/5
        public IActionResult Details(int? id)
        {
            IEnumerable<CoverType> objCoverTypeList = _UnitOfWork.CoverType.GetAll();
            if (id == null || objCoverTypeList == null)
            {
                return NotFound();
            }

            var coverType = objCoverTypeList.FirstOrDefault(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // GET: CoverType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CoverType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(coverType);
                _UnitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: CoverType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _UnitOfWork.CoverType.GetAll == null)
            {
                return NotFound();
            }

            var coverType = _UnitOfWork.CoverType.GetFirstOrDefault(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        // POST: CoverType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] CoverType coverType)
        {
            if (id != coverType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _UnitOfWork.CoverType.Update(coverType);
                    _UnitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoverTypeExists(coverType.Id))
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
            return View(coverType);
        }

        // GET: CoverType/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _UnitOfWork.CoverType.GetAll == null)
            {
                return NotFound();
            }

            var coverType = _UnitOfWork.CoverType.GetFirstOrDefault(m => m.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: CoverType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_UnitOfWork.CoverType.GetAll == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CoverTypes'  is null.");
            }
            var coverType = _UnitOfWork.CoverType.GetFirstOrDefault(m => m.Id == id);
            if (coverType != null)
            {
                _UnitOfWork.CoverType.Remove(coverType);
            }

            _UnitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool CoverTypeExists(int id)
        {
            if (_UnitOfWork.CoverType.GetFirstOrDefault(e => e.Id == id) == null)
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
