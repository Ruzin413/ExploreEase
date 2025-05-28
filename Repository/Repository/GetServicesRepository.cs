using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public bool UpdatePackagePrice(int tourpackageid, float price)
        {
            var result = _exploreEaseDbContext.TourPackage.FirstOrDefault(x => x.TourPackageId == tourpackageid);
            if (result != null)
            {
                result.price = price;
                _exploreEaseDbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeletePackageById(int tourPackageId)
        {
            var result = _exploreEaseDbContext.TourPackage.FirstOrDefault(x => x.TourPackageId == tourPackageId);
            if (result != null)
            {
                _exploreEaseDbContext.TourPackage.Remove(result);
                _exploreEaseDbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public Task<List<PaymentModel>> getPastPaymentByEmail(string email)
        {
            return _exploreEaseDbContext.Paymentdb.Where(x => x.email == email).ToListAsync();
        }
        public async Task<List<TourPackage>> GetTourPackages()
        {
            return await _exploreEaseDbContext.TourPackage.ToListAsync();
        }
    }
}
