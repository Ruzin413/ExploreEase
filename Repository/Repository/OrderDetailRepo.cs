using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Repository.Repository
{
    public class OrderDetailRepo
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public OrderDetailRepo(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext = exploreEaseDbContext;
        }
        public async Task<List<PaymentModel>> PaymentDetail(int id)
        {
            return await _exploreEaseDbContext.Paymentdb
                .Where(x => x.id == id)
                .ToListAsync();
        }
        public async Task<List<TourPackage>> TourDetail(int id)
        {
            var Tid = await _exploreEaseDbContext.Paymentdb
                .Where(x => x.id == id)
                .Select(x => x.Tourpackageid)
                .FirstOrDefaultAsync();
            var data = from a in _exploreEaseDbContext.Paymentdb
                       join b in _exploreEaseDbContext.TourPackage on a.Tourpackageid equals b.TourPackageId
                       where a.Tourpackageid == Tid
                       select new TourPackage
                       {
                           TourPackageId = b.TourPackageId,
                           Name = b.Name,
                           Description = b.Description,
                           Rating = b.Rating,
                           price = b.price,
                           Lat = b.Lat,
                           Long = b.Long,
                           Destination = b.Destination,
                           DestinationImage = b.DestinationImage,
                           NumberOfDays = b.NumberOfDays,
                       };
            return await data.ToListAsync();
        }
        public async Task<List<DayHotel>> BookingDayHotel(int id)
        {
            var Tid = await _exploreEaseDbContext.Paymentdb
                .Where(x => x.id == id)
                .Select(x => x.Tourpackageid)
                .FirstOrDefaultAsync();

            var data = from a in _exploreEaseDbContext.TourPackage
                       join B in _exploreEaseDbContext.dayHotels on a.TourPackageId equals B.TourPackageId
                       where a.TourPackageId == Tid
                       select new DayHotel
                       {
                           DayHotelId = B.DayHotelId,
                           TourPackageId = B.TourPackageId,
                           DayNumber = B.DayNumber,
                           HotelName = B.HotelName,
                           HotelDescription = B.HotelDescription,
                           HotelLocation = B.HotelLocation,
                       };

            return await data.ToListAsync();
        }
        public async Task<List<HotelImage>> BookingHotelImages(int id)
        {
            var Tid = await _exploreEaseDbContext.Paymentdb
                .Where(x => x.id == id)
                .Select(x => x.Tourpackageid)
                .FirstOrDefaultAsync();

            var data = from a in _exploreEaseDbContext.TourPackage
                       join B in _exploreEaseDbContext.dayHotels on a.TourPackageId equals B.TourPackageId
                       join c in _exploreEaseDbContext.hotelImage on B.DayHotelId equals c.DayHotelId
                       where a.TourPackageId == Tid
                       select new HotelImage
                       {
                           HotelImageId = c.HotelImageId,
                           DayNumber = c.DayNumber,
                           ImagePath = c.ImagePath,
                           DayHotelId = c.DayHotelId,
                       };

            return await data.ToListAsync();
        }
    }
}
