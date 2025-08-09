using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Repository;
using Services.Services;
using System.Diagnostics;
namespace ExploreEase.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ExploreEaseUser> _userManager;
        private readonly TourServices _tourServices;
        private readonly GetServices _getServices;
        private readonly OrderDetailServices _orderDetailServices;
        private readonly ReviewServices _reviewServices;
        public AdminController(UserManager<ExploreEaseUser> userManager, TourServices tourServices, GetServices getServices,OrderDetailServices orderDetailServices,ReviewServices reviewServices)
        {
            _userManager = userManager;
            _tourServices = tourServices;
            _getServices = getServices;
            _orderDetailServices = orderDetailServices;
            _reviewServices = reviewServices;
        }
        public async Task<IActionResult> Index()
        {
            int totalUsers = await _userManager.Users.CountAsync();
            int totalOrders = await _orderDetailServices.GetOrdersCountAsync(); // example using service
            float totalRevenue = await _orderDetailServices.GetTotalRevenueAsync(); // example using service
            int totalTours = await _orderDetailServices.GetToursCountAsync();
            var recentOrders = await _orderDetailServices.GetRecentOrdersAsync();
            var recentUsers = await GetRecentUsersByHighestIdAsync(5);
            var recentReviews = await _reviewServices.GetRecentReviewsAsync();
            ViewBag.RecentOrders = recentOrders;
            ViewBag.RecentUsers = recentUsers;
            ViewBag.RecentReviews = recentReviews;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalTours = totalTours;
            return View();
        }
        public async Task<List<ExploreEaseUser>> GetRecentUsersByHighestIdAsync(int count)
        {
            return await _userManager.Users
                .OrderByDescending(u => u.Id)  // highest ID = newest user assuming auto-increment
                .Take(count)
                .ToListAsync();
        }


        public IActionResult Test()
        {
            return View();
        }
        public IActionResult Users()
        {
            GetUser u1 = new GetUser(_userManager);
            var users = u1.GetUsers();

            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = new IdentityResult();
            if (user != null)
            {
                result = await _userManager.DeleteAsync(user);
            }
            return View(Users);
        }
        public IActionResult AddServices() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeletePackage(int tourPackageId)
        {
            var resutlt = _getServices.DeletePackageById(tourPackageId);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddServices(IFormCollection form)
        {
            var result = await _tourServices.InsertAllAsync(form);
            if (result.IsSuccess)
            {
                return RedirectToAction("AddServises");
            }
            else
            {
                return RedirectToAction("ErrorAddServices");
            }
        }
        public IActionResult ManageServices()
        {
            return View();
        }
        public IActionResult OrderList()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllPackages()
        {
            var model = await _getServices.GetTourPackages();
            return Json(model);
        }
        [HttpGet]
        public async Task<IActionResult> OrderList1()
        {
            var model = await _getServices.GetOrder();
            var model1 = await _getServices.GetOrder();
            return Json(model1);
        }
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = _getServices.DeleteOrderById(id);
            if (result)
                return Ok(); 
            return BadRequest("Delete failed.");
        }
        public async Task<IActionResult> UpdatePackagePrice(int tourPackageId, int updatedPrice)
        {
            var result =  _getServices.UpdatePackagePrice(tourPackageId, updatedPrice);
            if(result)
                return Ok();
            return BadRequest("Update Failed");
        }
        public async  Task<IActionResult> OrderDetail(int id)
        {
            var model = await _orderDetailServices.GetOrderDetail(id);
            return View(model);
        }
        public IActionResult AddServises()
        {
            return View();
        }
        public IActionResult ErrorAddServices()
        {
            return View();
        }

    } 
}
    

