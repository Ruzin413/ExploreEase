using DataAcessLayer.DataAcess;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class RecommendationRepo
    {
        private readonly ExploreEaseDbContext _context;

        // Constructor to inject the DbContext provided by the dependency injection container.
        public RecommendationRepo(ExploreEaseDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PaymentModel>> GetPaymentsByUsernameAsync(string username)
        {

            return await _context.Paymentdb
                                  .Where(p => p.email.ToLower() == username.ToLower()) 
                                  .OrderByDescending(p => p.BookingDate)
                                  .AsNoTracking() 
                                  .ToListAsync();
        }

        /// <summary>
        /// Fetches all tour packages from the database.
        /// </summary>
        /// <returns>An enumerable collection of TourPackage objects.</returns>
        public async Task<IEnumerable<TourPackage>> GetAllTourPackagesAsync()
        {
            // Use AsNoTracking() for read-only operations for performance.
            return await _context.TourPackage.AsNoTracking().ToListAsync();
        }

        // Add other data access methods as needed for other parts of your application.
        // Example:
        // public async Task<TourPackage> GetTourPackageByIdAsync(int id)
        // {
        //     return await _context.TourPackage.AsNoTracking().FirstOrDefaultAsync(tp => tp.TourPackageId == id);
        // }
        // public async Task AddPaymentAsync(PaymentModel payment)
        // {
        //     _context.Paymentdb.Add(payment);
        //     await _context.SaveChangesAsync();
        // }
    }
}
