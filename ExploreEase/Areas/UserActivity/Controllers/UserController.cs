using Microsoft.AspNetCore.Mvc;

namespace ExploreEase.Areas.UserActivity.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
