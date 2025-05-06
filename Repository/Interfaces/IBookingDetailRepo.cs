using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IBookingDetailRepo
    {
        List<TourPackage> BookingTourpackageRepo(int id);
        List<DayHotel> BookingDayHotel(int id);
        List<HotelImage> BookingHotelImages(int id);

    }
}
