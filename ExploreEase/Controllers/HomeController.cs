using System.ComponentModel;
using System.Diagnostics;
using ExploreEase.Models;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Services.Services;
namespace ExploreEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GetServices _getServices;

        public HomeController(ILogger<HomeController> logger, GetServices getServices)
        {
            _logger = logger;
            _getServices = getServices;
        }
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TourPackage()
        {
            var packages = await _getServices.GetTourPackages();
            return Json(packages);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
