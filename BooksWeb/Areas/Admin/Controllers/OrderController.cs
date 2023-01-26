using Books.DataAccess.Repository.IRepository;
using Books.Models;
using Books.Models.ViewModels;
using Books.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BooksWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {

            OrderVM = new OrderVM()
            {
                OrderHeader = _UnitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                OrderDetails = _UnitOfWork.OrderDetail.GetAll(u => u.OrderId == orderId, includeProperties: "Product")
            };
                

            return View(OrderVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
                {
                orderHeaders = _UnitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else 
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _UnitOfWork.OrderHeader.GetAll(u=> u.ApplicationUserId == claim.Value,includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
