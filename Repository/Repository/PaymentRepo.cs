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
