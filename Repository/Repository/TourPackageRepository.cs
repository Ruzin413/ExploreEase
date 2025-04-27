using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class TourRepository : ITourPackageRepository
    {
        private readonly ExploreEaseDbContext _dbContext;

        public TourRepository(ExploreEaseDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task InsertTourPackageWithHotelsAsync(TourPackage tourPackage, List<DayHotel> dayHotels, List<HotelImage> hotelImages)
        {
            await _dbContext.TourPackage.AddAsync(tourPackage);
            await _dbContext.SaveChangesAsync(); // Save to get TourPackageId

            foreach (var dayHotel in dayHotels)
            {
                dayHotel.TourPackageId = tourPackage.TourPackageId; // Set foreign key
                await _dbContext.dayHotels.AddAsync(dayHotel);
                await _dbContext.SaveChangesAsync(); // Save to get DayHotelId

                foreach (var hotelImage in hotelImages.Where(h => h.DayNumber == dayHotel.DayNumber))
                {
                    hotelImage.DayHotelId = dayHotel.DayHotelId; // Set foreign key
                    await _dbContext.hotelImage.AddAsync(hotelImage);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
