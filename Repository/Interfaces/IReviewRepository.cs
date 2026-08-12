using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IReviewRepository
    {
        Task<List<ReviewModel>> GetReviewsByTourPackageIdAsync(int tourPackageId);
        Task<TourPackage> GetTourPackageByIdAsync(int id);
        Task<bool> UpdateTourPackageAsync(TourPackage tourPackage);
        Task<bool> Insertintodb(int paymentId, string name, string email, int packageId, int rating, string reviewText);
        Task<List<ReviewModel>> GetRecentReviewsAsync(int count = 10);
    }
}
