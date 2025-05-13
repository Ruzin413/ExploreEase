using Models.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RecommendationService
    {
        private readonly RecommendationRepo _repository;
        public RecommendationService(RecommendationRepo repository)
        {
            _repository = repository;
        }
        /// <param name="username"
        public async Task<IEnumerable<TourPackage>> GetRecommendedTourPackagesAsync(string username)
        {
            var payments = await _repository.GetPaymentsByUsernameAsync(username);
            var latestPayment = payments.FirstOrDefault();
            if (latestPayment == null)
            {
                return new List<TourPackage>();
            }
            int latestBookedPackageId = latestPayment.Tourpackageid;
            float userLat = latestPayment.Latitude;
            float userLong = latestPayment.Longitude;
            var allTourPackages = await _repository.GetAllTourPackagesAsync();
            var recommendedPackagesWithDistance = allTourPackages
                .Where(tp => tp.TourPackageId != latestBookedPackageId) 
                .Select(tp => new 
                {
                    Package = tp,
                    Distance = CalculateDistance(userLat, userLong, tp.Lat, tp.Long)
                })
                .ToList(); 

            var sortedRecommendations = recommendedPackagesWithDistance
                .OrderBy(x => x.Distance)
                .ThenByDescending(x => x.Package.Rating) 
                .ToList();
            var topRecommendations = sortedRecommendations.Take(5).Select(x => x.Package).ToList();
            return topRecommendations;
        }
        private double CalculateDistance(float lat1, float lon1, float lat2, float lon2)
        {
            const double R = 6371e3; 
            var lat1Rad = lat1 * Math.PI / 180;
            var lat2Rad = lat2 * Math.PI / 180;
            var deltaLat = (lat2 - lat1) * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c / 1000; 
        }
    }
}
