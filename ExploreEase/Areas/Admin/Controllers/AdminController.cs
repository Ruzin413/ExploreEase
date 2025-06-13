using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public AdminController(UserManager<ExploreEaseUser> userManager, TourServices tourServices, GetServices getServices,OrderDetailServices orderDetailServices)
        {
            _userManager = userManager;
            _tourServices = tourServices;
            _getServices = getServices;
            _orderDetailServices = orderDetailServices;
        }
        public IActionResult Index()
        {
            return View();
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
    

