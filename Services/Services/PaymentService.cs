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
        public async Task<bool> InsertIntoPayment(IFormCollection form, string username ,string email)
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
                    TotalPrice = (float)totalPrice ,
                    extendedDate =false,
                    Reviewed = false
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
