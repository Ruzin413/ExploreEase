using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderDetailServices
    {
        Task<int> GetOrdersCountAsync();
        Task<float> GetTotalRevenueAsync();
        Task<List<PaymentModel>> GetRecentOrdersAsync(int count = 10);
        Task<int> GetToursCountAsync();
        Task<OrderDetailModel> GetOrderDetail(int id);
    }
}
