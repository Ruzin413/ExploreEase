using Models.Models;
using Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReviewServices
    {
        private readonly ReviewRepository _repo;
        public ReviewServices(ReviewRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> Insertintodb(int id, string name, string email, int packageId, int rating, string reviewText)
        {
            try
            {
                await _repo.Insertintodb(id, name, email, packageId, rating, reviewText);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateTourPackageRatingAsync(int packageId)
        {
            try
            {
                var reviews = await _repo.GetReviewsByTourPackageIdAsync(packageId);

                // Optional: Set rating to 0 if there are no reviews
                float avgRating = reviews.Count > 0 ? (float)reviews.Average(r => r.Rating) : 0f;
                var tourPackage = await _repo.GetTourPackageByIdAsync(packageId);
                tourPackage.Rating = avgRating;
                var updated = await _repo.UpdateTourPackageAsync(tourPackage);
                return updated; // Assume this returns a bool indicating success
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UpdateTourPackageRatingAsync: " + ex.Message);
                return false;
            }
        }
    }
}
