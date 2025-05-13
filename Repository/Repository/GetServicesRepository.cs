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
        public async Task<TourPackage?> GetTourPackageById(int id)
        {
            return await _exploreEaseDbContext.TourPackage.FirstOrDefaultAsync(x => x.TourPackageId == id);
        }
        public async Task<List<PaymentModel>> GetOrder()
        {
            return await _exploreEaseDbContext.Paymentdb.ToListAsync();
        }
        public bool DeleteOrderById(int id)
        {
            var order = _exploreEaseDbContext.Paymentdb.FirstOrDefault(x => x.id == id);
            if (order != null)
            {
                _exploreEaseDbContext.Paymentdb.Remove(order);
                _exploreEaseDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<List<TourPackage>> GetTourPackages()
        {
            return await _exploreEaseDbContext.TourPackage.ToListAsync();
        }
    }
}
