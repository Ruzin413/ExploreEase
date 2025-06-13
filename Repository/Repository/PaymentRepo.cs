using DataAcessLayer.DataAcess;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Repository.Repository
{
    public class PaymentRepo
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public PaymentRepo(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext = exploreEaseDbContext;
        }
        public async Task<bool> ExtendDate(int id, int numberOfDays)
        {
            try
            {
                var payment = await _exploreEaseDbContext.Paymentdb.FindAsync(id);
                if (payment == null) return false;
                payment.EndDate = payment.EndDate.AddDays(numberOfDays);
                payment.extendedDate = true;
                await _exploreEaseDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateReview(int id)
        {
            try
            {
                var payment = await _exploreEaseDbContext.Paymentdb.FindAsync(id);
                if (payment == null) return false;
                payment.Reviewed = true;
                await _exploreEaseDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> InsertIntoDB(PaymentModel model)
        {
            try
            {
                await _exploreEaseDbContext.Paymentdb.AddAsync(model);
                await _exploreEaseDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
