using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository.Repository
{
    public class ReviewRepository
    {
        private ExploreEaseDbContext _exploreEaseDbContext;
        public ReviewRepository(ExploreEaseDbContext exploreEaseDbContext)
        {
            _exploreEaseDbContext = exploreEaseDbContext;
        }

        public async Task<List<ReviewModel>> GetReviewsByTourPackageIdAsync(int tourPackageId)
        {
            return await _exploreEaseDbContext.Reviewdb
                .Where(r => r.TourPackageId == tourPackageId)
                .ToListAsync();
        }

        // Get a tour package by ID
        public async Task<TourPackage> GetTourPackageByIdAsync(int id)
        {
            return await _exploreEaseDbContext.TourPackage.FindAsync(id);
        }

        // Update a tour package (used to update the rating)
        public async Task<bool> UpdateTourPackageAsync(TourPackage tourPackage)
        {
            try
            {
_exploreEaseDbContext.TourPackage.Update(tourPackage);
            await _exploreEaseDbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public async Task<bool> Insertintodb(int paymentId, string name, string email, int packageId, int rating, string reviewText)
        {
            try
            {
                var review = new ReviewModel
                {
                    PaymentId = paymentId,
                    Name = name,
                    email = email,
                    TourPackageId = packageId,
                    Rating = rating,
                    Comment = reviewText,
                    CreatedAt = DateTime.Now
                };

                _exploreEaseDbContext.Reviewdb.Add(review);
                await _exploreEaseDbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Log exception
                return false;
            }
        }
    }
}
