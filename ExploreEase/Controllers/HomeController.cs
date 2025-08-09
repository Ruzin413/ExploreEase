using ExploreEase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Models.Models;
using NuGet.Protocol;
using Services.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
namespace ExploreEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GetServices _getServices;
        private readonly RecommendationService _recommendationService;
        private readonly UserManager<ExploreEaseUser> _userManager;
        private readonly BlogServices _blogservices;
        public HomeController(ILogger<HomeController> logger, GetServices getServices,RecommendationService recommendationService, UserManager<ExploreEaseUser> userManager,BlogServices blogServices)
        {
            _logger = logger;
            _getServices = getServices;
            _recommendationService = recommendationService; 
            _userManager = userManager;
            _blogservices = blogServices;   
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

            var username = user.Email;
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
        public IActionResult Blog()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostBlog(IFormCollection Form)
        {
            var user = await _userManager.GetUserAsync(User);
            var Email = user.Email;
            var name = user.UserName;
            bool  result = await  _blogservices.Postblog(Form,Email,name);
            if (result)
            {
                return Json(new { success = true});
            }
            else
            {
                return Json(new { success = true });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBlog()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }
            string userid = user.Email;
            var package = await _blogservices.GetBlogs(userid);
            return Json(package);
        }
        [HttpPost]
        public async Task<IActionResult> BlogLike(int blogId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }
            string userid = user.Email;
            bool result = await _blogservices.LikeUpdate(blogId, userid);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> BlogUnlike(int blogId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }
            string userid = user.Email;
            bool result = await _blogservices.unLikeUpdate(blogId, userid);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(int BlogId, string CommentText)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "User not logged in." });
            }
            string username = user.UserName;
            string email = user.Email;
            bool result = await _blogservices.PostComment(BlogId, username, email, CommentText);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetComments(int BlogId)
        {
            var data = await _blogservices.GetComments(BlogId);
            return Json(data);  
        }
    }
}
