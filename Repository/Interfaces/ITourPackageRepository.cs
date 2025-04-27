using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Repository.Interfaces
{
        public interface ITourPackageRepository
        {
        Task InsertTourPackageWithHotelsAsync(TourPackage tourPackage, List<DayHotel> dayHotels, List<HotelImage> hotelImages);
        }
}

