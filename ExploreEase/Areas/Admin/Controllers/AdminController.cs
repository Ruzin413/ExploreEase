using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repository.Repository;
using System.Diagnostics;
namespace ExploreEase.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ExploreEaseUser> _userManager;
        public AdminController(UserManager<ExploreEaseUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            GetUser u1 = new GetUser(_userManager);
            var users = u1.GetUsers();
            
            return View(users); 
        }
        [Area("Admin")]
        public IActionResult DeleteUser()
        {
            //var user = await _userManager.FindByIdAsync(id);
            //if (user != null)
            //{
            //    await _userManager.DeleteAsync(user);
            //}
            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            //var user = await _userManager.FindByIdAsync(id);
            //if (user != null)
            //{
            //    await _userManager.DeleteAsync(user);
            //}
            return View();
        }
    }
}
