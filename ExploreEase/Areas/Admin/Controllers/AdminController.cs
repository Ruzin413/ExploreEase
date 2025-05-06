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

        public AdminController(UserManager<ExploreEaseUser> userManager, TourServices tourServices)
        {
            _userManager = userManager;
            _tourServices = tourServices;
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
            return Json(result);
        }
        public IActionResult AddServices() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddServices(IFormCollection form)
        {
            var result = await _tourServices.InsertAllAsync(form);

            if (result.IsSuccess)
            {
                // Redirect to a temporary success page that will animate and redirect back
                return RedirectToAction("AddServises");
            }

            // Handle failure (e.g., redisplay form with error)
            TempData["Error"] = result.Message;
            return RedirectToAction("AddServices");
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
    

