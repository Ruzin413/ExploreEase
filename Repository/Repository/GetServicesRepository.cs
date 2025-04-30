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
    public class GetServicesRepository : IGetServicesRepository
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public GetServicesRepository(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext =  exploreEaseDbContext;
        }

        public Task<DayHotel> GetDayHotel()
        {
            throw new NotImplementedException();
        }

        public Task<HotelImage> GetHotelImage()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TourPackage>> GetTourPackages()
        {
            return await _exploreEaseDbContext.TourPackage.ToListAsync();
        }
    }
}
