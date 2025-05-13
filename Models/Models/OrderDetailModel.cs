using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class OrderDetailModel
    {
        public List<PaymentModel> PaymentModel { get; set; }
        public List<TourPackage> TourPackages { get; set; }
        public List<DayHotel> DayHotels { get; set; }
        public List<HotelImage> HotelImages { get; set; }
    }
}
