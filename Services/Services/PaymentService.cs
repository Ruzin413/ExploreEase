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
        public PaymentService(PaymentRepo repo){
            _repo = repo;   
        }
        public async Task<bool> InsertIntoPayment(IFormCollection form, string username)
        {
            try
            {
                // Parse form data
                var startDate = DateOnly.Parse(form["TourDate"]);
                var numberOfDays = int.Parse(form["NumberOfDays"]);
                var numberOfPeople = int.Parse(form["NumberOfPeople"]); // Number of people
                var pricePerPerson = Convert.ToInt32(form["price"]); // Price per person
                // Calculate total price
                var totalPrice = pricePerPerson * numberOfPeople; // Total price based on number of people
                var data = new PaymentModel
                {
                    username = username,
                    Tourpackageid = Convert.ToInt32(form["tourPackageId"]),
                    price = pricePerPerson,// Price per person (not the total price)
                    Rating = float.Parse(form["rating"]),
                    StartDate = startDate,
                    EndDate = startDate.AddDays(numberOfDays),
                    BookingDate = DateTime.Now,
                    Latitude = float.Parse(form["Latitude"]),
                    Longitude = float.Parse(form["Longitude"]),
                    NumberOfPeople = numberOfPeople, // Set the number of people
                    TotalPrice = totalPrice 
                };
                return await _repo.InsertIntoDB(data);
            }
            catch
            {
                return false;
            }
        }
    }
}
