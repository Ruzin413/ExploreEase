using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;
using Repository.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class OrderDetailServices : IOrderDetailServices
    {
        private readonly IOrderDetailRepo _orderDetailRepo;
        public OrderDetailServices(IOrderDetailRepo orderDetailRepo)
        {
            _orderDetailRepo = orderDetailRepo;
        }
        public async Task<int> GetOrdersCountAsync()
        {
            return await _orderDetailRepo.GetOrdersCountAsync();
        }

        public async Task<float> GetTotalRevenueAsync()
        {
            return await _orderDetailRepo.GetTotalRevenueAsync();
        }
        public async Task<List<PaymentModel>> GetRecentOrdersAsync(int count = 10)
        {
            return await _orderDetailRepo.GetRecentOrdersAsync();
        }
        
        public async Task<int> GetToursCountAsync()
        {
            return await _orderDetailRepo.GetToursCountAsync();
        }
        public async Task<OrderDetailModel> GetOrderDetail(int id)
        {
            var packages = new OrderDetailModel
            {
                PaymentModel = await _orderDetailRepo.PaymentDetail(id), // await this first
                TourPackages = await _orderDetailRepo.TourDetail(id),    // await this second
                DayHotels = await _orderDetailRepo.BookingDayHotel(id),  // await this third
                HotelImages = await _orderDetailRepo.BookingHotelImages(id), // await this last
            };
            return packages;
        }

    }
}
