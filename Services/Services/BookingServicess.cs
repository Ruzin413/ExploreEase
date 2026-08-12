using DataAcessLayer.DataAcess;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Repository.Interfaces;
using Repository.Repository;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class BookingServicess : IBookingServices
    {
        private readonly IBookingRepo _repository;
        public BookingServicess(IBookingRepo bookingRepo)
        {
            _repository = bookingRepo;
        }
        public async Task<bool> AddBookmark(string email, int tourPackageId)
        {
            var result = await _repository.AddBookmark(email, tourPackageId);
            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> IsBookmarked(string email, int tourPackageId)
        {
            return await _repository.IsBookmarked(email, tourPackageId);
        }
        public async Task<bool> RemoveBookmark(string email, int tourPackageId)
        {
            return await _repository.RemoveBookmark(email, tourPackageId);  
        }
        public async Task<List<TourPackage>> GetBookmarkedTourPackages(string email)
        {
            return await _repository.GetBookmarkedTourPackages(email);
        }
    }
}
