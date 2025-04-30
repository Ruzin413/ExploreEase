using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ImageSaveService : IimageService
    {
        public async Task<string> SaveImageAsync(IFormFile file, string folderName)
        {
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var targetFolder = Path.Combine(rootPath, folderName);
            Directory.CreateDirectory(targetFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(targetFolder, uniqueFileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("/images", folderName, uniqueFileName).Replace("\\", "/");
        }
    }
}
