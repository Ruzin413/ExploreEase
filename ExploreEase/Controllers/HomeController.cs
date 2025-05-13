using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using ExploreEase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Services.Services;
namespace ExploreEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GetServices _getServices;
        private readonly RecommendationService _recommendationService;
        private readonly UserManager<ExploreEaseUser> _userManager;
        public HomeController(ILogger<HomeController> logger, GetServices getServices,RecommendationService recommendationService, UserManager<ExploreEaseUser> userManager)
        {
            _logger = logger;
            _getServices = getServices;
            _recommendationService = recommendationService; 
            _userManager = userManager;
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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TourPackage>>> GetRecommendationsForCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null || string.IsNullOrEmpty(user.FullName))
            {
                return Unauthorized("User not authenticated or FullName not available.");
            }

            var username = user.FullName;
            var recommendations = await _recommendationService.GetRecommendedTourPackagesAsync(username);

            if (recommendations == null || !recommendations.Any())
            {
                return Ok(new List<TourPackage>());
            }
            return Ok(recommendations);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
