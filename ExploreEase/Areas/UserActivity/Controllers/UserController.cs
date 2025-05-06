using Microsoft.AspNetCore.Mvc;
using Models.Models;
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
        public IActionResult ShowLocation()
        {

            return View();
        }

        [HttpPost]
        [Area("UserActivity")]
        public IActionResult ShowLocation(double lat, double longi, string destination)
        {
            ViewData["Latitude"] = lat;
            ViewData["Longitude"] = longi;
            ViewData["Destination"] = destination;
            return View();
        }


    }
}
