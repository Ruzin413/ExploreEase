using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace ExploreEase.Areas.UserActivity.Controllers
{
    [Area("UserActivity")]
    public class UserController : Controller
    {
        private readonly BookingDetails _bookingDetails;
        public UserController(BookingDetails bookingDetails)
        {
            _bookingDetails = bookingDetails;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Booking(int id)
        {
            var model = _bookingDetails.GetTourPackages(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult ShowLocation(double lat, double @long)
        { 
            ViewBag.Latitude = lat;
            ViewBag.Longitude = @long;
            return View();
        }
    }
}
