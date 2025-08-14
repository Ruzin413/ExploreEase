using Microsoft.AspNetCore.Http;
using Models.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PaymentService
    {
        private readonly PaymentRepo _repo;
        private readonly EmailServices _emailServices;  
        public PaymentService(PaymentRepo repo,EmailServices emailServices){
            _repo = repo;   
            _emailServices = emailServices;

        }
        public async Task<bool> ExtendDate(int id,int numb)
        {
            try
            {
                await _repo.ExtendDate(id, numb);
                return true;
            }
            catch
            {
                return false;
            }
           
        }
        public async Task<bool> UpdateReview(int id )
        {
            try
            {
               await  _repo.UpdateReview(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> InsertIntoPayment(IFormCollection form, string username, string email)
        {
            try
            {
                var startDate = DateOnly.Parse(form["TourDate"]);
                var numberOfDays = int.Parse(form["NumberOfDays"]);
                var numberOfPeople = int.Parse(form["NumberOfPeople"]);
                var pricePerPerson = Convert.ToDouble(form["price"]);
                var totalPrice = pricePerPerson * numberOfPeople;

                var data = new PaymentModel
                {
                    username = username,
                    email = email,
                    Tourpackageid = Convert.ToInt32(form["tourPackageId"]),
                    price = (float)pricePerPerson,
                    Rating = float.Parse(form["rating"]),
                    StartDate = startDate,
                    EndDate = startDate.AddDays(numberOfDays),
                    BookingDate = DateTime.Now,
                    Latitude = float.Parse(form["Latitude"]),
                    Longitude = float.Parse(form["Longitude"]),
                    NumberOfPeople = numberOfPeople,
                    TotalPrice = (float)totalPrice,
                    extendedDate = false,
                    Reviewed = false
                };

                bool state = await _repo.InsertIntoDB(data);
                if (state)
                {
                    var subject = "ExploreEase: Your Tour Booking Confirmation";
                    var body = $@"
                <p>Dear {username},</p>
                <p>Thank you for booking with ExploreEase!</p>
                <p>Your tour starting on <strong>{startDate:MMMM dd, yyyy}</strong> for <strong>{numberOfPeople} person(s)</strong> has been successfully confirmed.</p>
                <p><strong>Total Price:</strong> Rs{totalPrice:F2}</p>
                <p>Please review the details of your order below for important information, including the meeting point and instructions for your tour day.</p>
                <br />
                <p>We hope you have a fantastic experience. If you have any questions, feel free to contact us.</p>
                <br />
                <p>Best regards,<br />The ExploreEase Team</p>
            ";
                    var emailSent = _emailServices.SendEmail(email, subject, body);
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

    }
}
