using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGetServices
    {
        Task<List<TourPackage>> GetTourPackages();
        Task<TourPackage> GetTourPackageById(int id);
        Task<List<TourPackage>> GetTourPackageByName(string name);
        Task<List<PaymentModel>> GetOrder();
        Task<List<PaymentModel>> GetorderByEmail(string email);
        Task<PaymentModel> GetorderById(int id);
        Task<List<PaymentModel>> getPastPaymentByEmail(string email);
        bool DeleteOrderById(int id);
        bool DeletePackageById(int tourPackageId);
        bool UpdatePackagePrice(int tourpackageId, int price);
    }
}
