using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReviewServices
    {
        Task<bool> Insertintodb(int id, string name, string email, int packageId, int rating, string reviewText);
        Task<List<ReviewModel>> GetReviewfromtourpackageid(int tid);
        Task<bool> UpdateTourPackageRatingAsync(int packageId);
        Task<List<ReviewModel>> GetRecentReviewsAsync();
    }
}
