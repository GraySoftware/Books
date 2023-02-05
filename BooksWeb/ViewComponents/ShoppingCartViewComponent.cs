using Books.DataAccess.Repository.IRepository;
using Books.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                {
                    // session is already set so jsut retrieve value and return
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
                else
                {
                    // session is not set so we have to go to the database and retrieve the count
                    HttpContext.Session.SetInt32(SD.SessionCart,
                        _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }
            }
            else
            {
                // user is not logged in for some reason so its safe to clear the session
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
