using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BookingDetails
    {
        private readonly BookingDetailRepo _bookingDetailRepo;
        public BookingDetails(BookingDetailRepo bookingDetailRepo)
        {
            _bookingDetailRepo = bookingDetailRepo; 
        }
        public  BookingServices GetTourPackages(int id)
        {
            var packages = new BookingServices
            {
                TourPackages =  _bookingDetailRepo.BookingTourpackageRepo(id),
                DayHotels =     _bookingDetailRepo.BookingDayHotel(id),
                HotelImages =  _bookingDetailRepo.BookingHotelImages(id),
            };
            return packages; 
        }
    }
}
