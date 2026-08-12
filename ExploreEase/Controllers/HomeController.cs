using ExploreEase.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Services.Interfaces;
using Services.Services;
using System.Diagnostics;

namespace ExploreEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetServices _getServices;
        private readonly IRecommendationService _recommendationService;
        private readonly UserManager<ExploreEaseUser> _userManager;
        private readonly IBlogServices _blogservices;

        public HomeController(
            ILogger<HomeController> logger,
            IGetServices getServices,
            IRecommendationService recommendationService,
            UserManager<ExploreEaseUser> userManager,
            IBlogServices blogServices)
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
                TempData["Message"] = "Log in to see your recommendations.";
                return Redirect("/Identity/Account/Login");
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
            if (user == null)
            {
                TempData["Message"] = "Log in to create a blog post.";
                return Redirect("/Identity/Account/Login");
            }

            var Email = user.Email;
            var name = user.UserName;
            bool result = await _blogservices.Postblog(Form, Email, name);

            return Json(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetBlog()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Log in to view your blogs.";
                return Redirect("/Identity/Account/Login");
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
                TempData["Message"] = "Log in to like a blog.";
                return Redirect("/Identity/Account/Login");
            }
            string userid = user.Email;
            bool result = await _blogservices.LikeUpdate(blogId, userid);
            var blog = await _blogservices.GetBlogs(userid);
            var currentBlog = blog.FirstOrDefault(b => b.Id == blogId);
            int likeCount = currentBlog?.Likes ?? 0;
            return Json(new
            {
                success = result,
                likes = likeCount,
                liked = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> BlogUnlike(int blogId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Log in to unlike a blog.";
                return Redirect("/Identity/Account/Login");
            }

            string userid = user.Email;
            bool result = await _blogservices.unLikeUpdate(blogId, userid);

            var blog = await _blogservices.GetBlogs(userid);
            var currentBlog = blog.FirstOrDefault(b => b.Id == blogId);
            int likeCount = currentBlog?.Likes ?? 0;

            return Json(new
            {
                success = result,
                likes = likeCount,
                liked = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(int BlogId, string CommentText)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Log in to post a comment.";
                return Redirect("/Identity/Account/Login");
            }

            string username = user.UserName;
            string email = user.Email;
            bool result = await _blogservices.PostComment(BlogId, username, email, CommentText);

            return Json(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetComments(int BlogId)
        {
            var data = await _blogservices.GetComments(BlogId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return View(new List<TourPackage>());
            }
            var data = await _getServices.GetTourPackageByName(name);
            ViewBag.SearchQuery = name;
            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var result = await _blogservices.DeleteBlog(blogId);
            if (result)
            {
                return Json( new {  success = true} );
            }
            else
            {
                return Json(new
                {
                    success = false,
                });
            }
        }

    }
}
