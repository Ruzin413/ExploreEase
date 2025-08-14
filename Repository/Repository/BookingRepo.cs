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
    public class BookingRepo
    {
        private readonly ExploreEaseDbContext _exploreEaseDbContext;
        public BookingRepo(ExploreEaseDbContext exploreEaseDbContext)
        {
        _exploreEaseDbContext = exploreEaseDbContext;            
        }
        public async Task<bool> AddBookmark(string email, int tourPackageId)
        {
            var data = new BookmarkModel
            {
                userEmail = email,
                TourPackageId = tourPackageId
            };

            await _exploreEaseDbContext.Bookmarkdb.AddAsync(data);

            var result = await _exploreEaseDbContext.SaveChangesAsync();

            return result > 0; 
        }
        public async Task<bool> IsBookmarked(string email, int tourPackageId)
        {
            return await _exploreEaseDbContext.Bookmarkdb
                .AnyAsync(b => b.userEmail == email && b.TourPackageId == tourPackageId);
        }
        public async Task<bool> RemoveBookmark(string email, int tourPackageId)
        {
            var bookmark = await _exploreEaseDbContext.Bookmarkdb
                .FirstOrDefaultAsync(b => b.userEmail == email && b.TourPackageId == tourPackageId);

            if (bookmark == null)
                return false; // nothing to remove

            _exploreEaseDbContext.Bookmarkdb.Remove(bookmark);

            var result = await _exploreEaseDbContext.SaveChangesAsync();

            return result > 0; // true if a row was deleted
        }
        public async Task<List<TourPackage>> GetBookmarkedTourPackages(string email)
        {
            var tourPackageIds = await _exploreEaseDbContext.Bookmarkdb
                .Where(b => b.userEmail == email)
                .Select(b => b.TourPackageId)
                .ToListAsync();
            var packages = await _exploreEaseDbContext.TourPackage
                .Where(tp => tourPackageIds.Contains(tp.TourPackageId))
                .ToListAsync();
            return packages;
        }
    }
}
