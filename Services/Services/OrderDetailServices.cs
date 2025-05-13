using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;

namespace Services.Services
{
    public class OrderDetailServices
    {
        private readonly OrderDetailRepo _orderDetailRepo;
        public OrderDetailServices(OrderDetailRepo orderDetailRepo)
        {
            _orderDetailRepo = orderDetailRepo;
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
