using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;
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
        public UserController(BookingDetails bookingDetails, GetServices getServices, UserManager<ExploreEaseUser> userManager, KhaltiService khaltiService,PaymentService paymentService, ReviewServices reviewServices)
        {
            _bookingDetails = bookingDetails;
            _getServices = getServices;
            _userManager = userManager;
            _khaltiService = khaltiService;
            _paymentService = paymentService;
            _reviewServices = reviewServices;
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
        [HttpPost]
        [Area("UserActivity")]
        public async Task<IActionResult> BookServices(int tourPackageId)
        {
            var model = await _getServices.GetTourPackageById(tourPackageId);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [HttpPost]
        [Area("UserActivity")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> OrderPackage(int tourPackageId)
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;
            
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
        //[Authorize(Roles = "User")]
        public IActionResult PaymentSucess(){
            return View();
        }
        [Authorize(Roles = "User")]
        public IActionResult PaymentError()
        {
            return View();
        }
        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> Payment(IFormCollection Form)
        {
            var user = await _userManager.GetUserAsync(User);
            var username = user?.FullName;
            var email = user?.Email;   
            var result = await _paymentService.InsertIntoPayment(Form, username,email);
            if (result)
            {
                return View("PaymentSucess");
            }
            else
            {
                return View("PaymentError");
            }
        }
        public async Task<IActionResult> UserOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = user?.Email;
           var model = await _getServices.GetorderByEmail(email);
            return View(model);
        }

        [HttpPost]
        public async  Task<bool> ExtendDate(int TourPackageId, int id)
        {
           var data = await _getServices.GetTourPackageById(TourPackageId);
            var numb = data.NumberOfDays;
            var sucess = await  _paymentService.ExtendDate(id,numb);
            return sucess;
        }
        [HttpPost]
        public async Task<IActionResult> SubmitReview(int id, int packageId, int rating, string reviewText)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { success = false, error = "User not found" });

            var email = user.Email;
            var name = user.UserName;

            try
            {
                // Insert review first
                var inserted = await _reviewServices.Insertintodb(id, name, email, packageId, rating, reviewText);
                if (!inserted)
                    return Json(new { success = false, error = "Failed to insert review" });

                // Update package rating
                var updatedRating = await _reviewServices.UpdateTourPackageRatingAsync(packageId);
                if (!updatedRating)
                    return Json(new { success = false, error = "Failed to update rating" });

                // Mark payment as reviewed
                await _paymentService.UpdateReview(id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log ex
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> InitiateKhaltiPayment([FromBody] KhaltiBookingDTO dto)
        {
            var user = await _userManager.GetUserAsync(User);
            var request = new KhaltiRequest
            {
                return_url = Url.Action("Index", "Home", null, Request.Scheme),
                website_url = "https://localhost:7285/",
                amount = (long)dto.Price * 100, // Khalti expects paisa
                purchase_order_id = $"PKG{dto.TourPackageId}_{DateTime.Now.Ticks}",
                purchase_order_name = dto.PackageName,
                customer_info = new CustomerInfo
                {
                    name = user?.FullName ?? "Guest",
                    email = user?.Email ?? "guest@example.com",
                    phone = user?.PhoneNumber ?? "9800000000"
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
                // Optionally log ex
                return BadRequest(new { message = "Failed to parse Khalti response.", error = ex.Message });
            }

            return Json(new { payment_url = result?.payment_url?.ToString() });
        }
    }
    }
