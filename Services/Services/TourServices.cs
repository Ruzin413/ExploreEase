using DataAcessLayer.DataAcess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Repository;
using Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Services.Services
{
    public class TourServices : ITourServices
    {
        private readonly ImageSaveService _imageSaveService;
        private readonly TourRepository _tourRepository;

        public TourServices( ImageSaveService imageSaveService, TourRepository tourRepository)
        {
            _imageSaveService = imageSaveService;
            _tourRepository = tourRepository;
        }

        public async Task<Result> InsertAllAsync(IFormCollection form)
        {
            try
            {
                var name = form["Name"];
                var description = form["Description"];
                var dayNumber = Convert.ToInt32(form["DayNumber"]);
                var tourPackage = new TourPackage
                {
                    Name = name,
                    Description = description,
                    Rating  = 1 
                };
                var dayHotels = new List<DayHotel>();
                var hotelImages = new List<HotelImage>();
                for (int i = 1; i <= dayNumber; i++)
                {
                    var hotelName = form[$"HotelName_{i}"];
                    var hotelDescription = form[$"HotelDescription_{i}"];
                    var hotelLocation = form[$"HotelLocation_{i}"];

                    var dayHotel = new DayHotel
                    {
                        DayNumber = i,
                        HotelName = hotelName,
                        HotelDescription = hotelDescription,
                        HotelLocation = hotelLocation
                    };
                    dayHotels.Add(dayHotel);
                    var files = form.Files.Where(f => f.Name == $"HotelImage_{i}[]");
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var savedPath = await _imageSaveService.SaveImageAsync(file, "HotelImages");

                            var hotelImage = new HotelImage
                            {
                                ImagePath = savedPath,
                                DayNumber = i
                            };
                            hotelImages.Add(hotelImage);
                        }
                    }
                }
                await _tourRepository.InsertTourPackageWithHotelsAsync(tourPackage, dayHotels, hotelImages);
                return new Result
                {
                    IsSuccess = true,
                    Message = "Tour Package, Day Hotels, and Hotel Images inserted successfully."
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

    }

}


