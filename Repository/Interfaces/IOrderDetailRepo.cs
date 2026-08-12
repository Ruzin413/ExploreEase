using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IOrderDetailRepo
    {
        Task<List<PaymentModel>> PaymentDetail(int id);
        Task<List<TourPackage>> TourDetail(int id);
        Task<List<DayHotel>> BookingDayHotel(int id);
        Task<List<HotelImage>> BookingHotelImages(int id);
        Task<int> GetOrdersCountAsync();
        Task<float> GetTotalRevenueAsync();
        Task<int> GetToursCountAsync();
        Task<List<PaymentModel>> GetRecentOrdersAsync(int count = 10);
    }
}
