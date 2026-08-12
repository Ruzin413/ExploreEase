using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IBookingRepo
    {
        Task<bool> AddBookmark(string email, int tourPackageId);
        Task<bool> IsBookmarked(string email, int tourPackageId);
        Task<bool> RemoveBookmark(string email, int tourPackageId);
        Task<List<TourPackage>> GetBookmarkedTourPackages(string email);
    }
}
