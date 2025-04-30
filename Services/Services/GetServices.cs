using DataAcessLayer.DataAcess;
using Models.Models;
using Repository.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class GetServices : IGetServices
    {
        private readonly GetServicesRepository _getServicesRepository;
        public GetServices(GetServicesRepository getServicesRepository){
            _getServicesRepository = getServicesRepository;
        }

        public Task<DayHotel> GetDayHotel()
        {
            throw new NotImplementedException();
        }

        public Task<HotelImage> GetHotelImage()
        {
            throw new NotImplementedException();
        }

        public async Task<List<TourPackage>> GetTourPackages()
        {
            return await _getServicesRepository.GetTourPackages();
        }
    }
}
