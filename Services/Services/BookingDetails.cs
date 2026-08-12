using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Models;
using Repository.Interfaces;
using Repository.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BookingDetails : IBookingDetails
    {
        private readonly IBookingDetailRepo _bookingDetailRepo;
        public BookingDetails(IBookingDetailRepo bookingDetailRepo)
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
