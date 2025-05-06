using DataAcessLayer.DataAcess;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class BookingDetailRepo
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public BookingDetailRepo(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext = exploreEaseDbContext;
        }
        public List<TourPackage> BookingTourpackageRepo(int id)
        {
            var data = _exploreEaseDbContext.TourPackage.Where(x => x.TourPackageId == id).ToList();
            return data;
        }
        public List<DayHotel> BookingDayHotel(int id)
        {
            var data = from a in _exploreEaseDbContext.TourPackage
                       join B in _exploreEaseDbContext.dayHotels on a.TourPackageId equals B.TourPackageId where a.TourPackageId == id
                       select new DayHotel
                       {
                           DayHotelId = B.DayHotelId,
                           TourPackageId= B.TourPackageId,
                           DayNumber = B.DayNumber,
                           HotelName = B.HotelName,
                           HotelDescription = B.HotelDescription,
                           HotelLocation = B.HotelLocation,
                       };
            return data.ToList();
        }
        public List<HotelImage> BookingHotelImages(int id)
        {
            var data = from a in _exploreEaseDbContext.TourPackage
                       join B in _exploreEaseDbContext.dayHotels on a.TourPackageId equals B.TourPackageId
                       join c in _exploreEaseDbContext.hotelImage on B.DayHotelId equals c.DayHotelId
                       where a.TourPackageId == id
                       select new HotelImage
                       {
                           HotelImageId = c.HotelImageId,
                           DayNumber = c.DayNumber,
                           ImagePath = c.ImagePath,
                           DayHotelId = c.DayHotelId,
                       };
            return data.ToList();
        }
    }
}
