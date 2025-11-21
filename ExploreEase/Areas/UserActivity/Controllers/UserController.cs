using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Models.Models;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreEase.Areas.UserActivity.Controllers
{
    [Area("UserActivity")]
    public class UserController : Controller
    {
        private readonly BookingDetails _bookingDetails;
        private readonly GetServices _getServices;
        private readonly UserManager<ExploreEaseUser> _userManager;
        private readonly KhaltiService _khaltiService;
        private readonly PaymentService _paymentService;
        private readonly ReviewServices _reviewServices;
        private readonly BookingServicess _bookingService;

        public UserController(
            BookingDetails bookingDetails,
            GetServices getServices,
            UserManager<ExploreEaseUser> userManager,
            KhaltiService khaltiService,
            PaymentService paymentService,
            ReviewServices reviewServices,BookingServicess bookingService)
        {
            _bookingDetails = bookingDetails;
            _getServices = getServices;
            _userManager = userManager;
            _khaltiService = khaltiService;
            _paymentService = paymentService;
            _reviewServices = reviewServices;
            _bookingService = bookingService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Booking(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;

            // Check if bookmarked
            ViewBag.Bookmarked = false;
            if (!string.IsNullOrEmpty(email))
            {
                ViewBag.Bookmarked = await _bookingService.IsBookmarked(email, id);
            }

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
        public IActionResult ShowLocation2()
        {
            return View();
        }
        [HttpPost]
        [Area("UserActivity")]
        public IActionResult ShowLocation2(double lat, double longi, string destination)
        {
            ViewData["Latitude"] = lat;
            ViewData["Longitude"] = longi;
            ViewData["Destination"] = destination;
            return View();
        }
        [HttpPost]
        [Area("UserActivity")]
        public async Task<IActionResult> BookServices(int tourPackageId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to book services.";
                return Redirect("/Identity/Account/Login");
            }

            var model = await _getServices.GetTourPackageById(tourPackageId);
            if (model == null)
                return NotFound();
            return View(model);
        }

        [HttpPost]
        [Area("UserActivity")]
        public async Task<IActionResult> OrderPackage(int tourPackageId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to order a package.";
                return Redirect("/Identity/Account/Login");
            }

            var email = user.Email;
            var pastPayments = await _getServices.getPastPaymentByEmail(email);
            var latestPayment = pastPayments
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            if (latestPayment != null)
            {
                ViewBag.StartDate = latestPayment.StartDate.ToString("yyyy-MM-dd");
                ViewBag.EndDate = latestPayment.EndDate.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.StartDate = null;
                ViewBag.EndDate = null;
            }
            var model = await _getServices.GetTourPackageById(tourPackageId);
            return View(model);
        }

        public IActionResult PaymentSucess()
        {
            return View();
        }

        public IActionResult PaymentError()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Payment(IFormCollection Form)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to complete the payment.";
                return Redirect("/Identity/Account/Login");
            }
            var username = user.FullName;
            var email = user.Email;
            var result = await _paymentService.InsertIntoPayment(Form, username, email);
            if (result)
            {

                return View("PaymentSucess");
            }
            else
            {
                return View("PaymentError");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowReview(int Tourpackageid)
        {
            var model = await _reviewServices.GetReviewfromtourpackageid(Tourpackageid);
            return Json(model);
        }

        public async Task<IActionResult> UserOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to view your orders.";
                return Redirect("/Identity/Account/Login");
            }

            var email = user.Email;
            var model = await _getServices.GetorderByEmail(email);
            return View(model);
        }

        [HttpPost]
        public async Task<bool> ExtendDate(int TourPackageId, int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Since this is not a View return, just return false
                return false;
            }

            var data = await _getServices.GetTourPackageById(TourPackageId);
            var numb = data.NumberOfDays;
            var success = await _paymentService.ExtendDate(id, numb);
            return success;
        }
        public async Task<IActionResult> OrderDetails(int id)
        {
            var payment = await _getServices.GetorderById(id);
            if (payment == null)
            {
                return NotFound();
            }

            var package = await _getServices.GetTourPackageById(payment.Tourpackageid);
            if (package == null)
            {
                return NotFound();
            }
            ViewBag.paymentid = id;
            ViewBag.Startdate = payment.StartDate;
            ViewBag.NumberOfPeople = payment.NumberOfPeople;

            // Create a combined model for the view (adjust names/types accordingly)
            var viewModel = new
            {
                Payment = payment,
                TourPackage = package
            };

            return View(viewModel);  // Pass the combined model
        }

        [HttpPost]
        public async Task<IActionResult> SubmitReview(int id, int packageId, int rating, string reviewText)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to submit a review.";
                return Redirect("/Identity/Account/Login");
            }

            var email = user.Email;
            var name = user.UserName;

            try
            {
                var inserted = await _reviewServices.Insertintodb(id, name, email, packageId, rating, reviewText);
                if (!inserted)
                    return Json(new { success = false, error = "Failed to insert review" });

                var updatedRating = await _reviewServices.UpdateTourPackageRatingAsync(packageId);
                if (!updatedRating)
                    return Json(new { success = false, error = "Failed to update rating" });

                await _paymentService.UpdateReview(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> InitiateKhaltiPayment([FromBody] KhaltiBookingDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to initiate payment.";
                return Redirect("/Identity/Account/Login");
            }

            var request = new KhaltiRequest
            {
                return_url = Url.Action("Index", "Home", null, Request.Scheme),
                website_url = "https://localhost:7285/",
                amount = (long)dto.Price * 100,
                purchase_order_id = $"PKG{dto.TourPackageId}_{DateTime.Now.Ticks}",
                purchase_order_name = dto.PackageName,
                customer_info = new CustomerInfo
                {
                    name = user.FullName,
                    email = user.Email,
                    phone = user.PhoneNumber ?? "9800000000"
                }
            };

            var resultJson = await _khaltiService.InitiatePaymentAsync(request);
            if (string.IsNullOrWhiteSpace(resultJson))
            {
                return BadRequest(new { message = "Failed to initiate payment. No response from Khalti." });
            }

            dynamic result;
            try
            {
                result = JsonConvert.DeserializeObject(resultJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to parse Khalti response.", error = ex.Message });
            }

            return Json(new { payment_url = result?.payment_url?.ToString() });
        }
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddBookmark(int tourPackageId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Message"] = "Please log in to add book mark.";
                return Redirect("/Identity/Account/Login");
            }
            var email = user.Email;
            var result = await _bookingService.AddBookmark( email, tourPackageId);
            if (result)
            {
                return Json(new { success = true });
            }
            else { 
                return Json(new
                {
                    success = false,
                });
            }
        }
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<IActionResult> RemoveBookmark(int tourPackageId)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user.Email;
            var result = await _bookingService.RemoveBookmark(email, tourPackageId);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new
                {
                    success = false,
                });
            }
        }
        public async  Task<IActionResult> Bookmarks()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user.Email;
            var Model =await _bookingService.GetBookmarkedTourPackages(email);
            return View(Model);
        }
    }
}
