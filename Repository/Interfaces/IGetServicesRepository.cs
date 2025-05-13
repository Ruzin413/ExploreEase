using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public  interface IGetServicesRepository
    {
        Task<List<TourPackage>> GetTourPackages();
        Task<TourPackage?> GetTourPackageById(int id);
    }
}
