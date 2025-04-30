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
          Task<DayHotel> GetDayHotel();
          Task<HotelImage> GetHotelImage();
    }
}
