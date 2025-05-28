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
        public Task<List<PaymentModel>> GetOrder()
        {
            return _getServicesRepository.GetOrder();
        }
        public Task<TourPackage> GetTourPackageById(int id)
        {
            return _getServicesRepository.GetTourPackageById(id);
        }
        public bool DeleteOrderById(int id)
        {
            return _getServicesRepository.DeleteOrderById(id);
        }
        public bool UpdatePackagePrice(int tourpackageId,int price)
        {
           return  _getServicesRepository.UpdatePackagePrice(tourpackageId, price);
        }
        public bool DeletePackageById(int tourPackageId)
        {
            return _getServicesRepository.DeletePackageById(tourPackageId);
        }
        public async Task<List<PaymentModel>> getPastPaymentByEmail(string email)
        {
            return await _getServicesRepository.getPastPaymentByEmail(email);
        }
        public async Task<List<TourPackage>> GetTourPackages()
        {
            return await _getServicesRepository.GetTourPackages();
        }
    }
}