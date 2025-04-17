using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models.Models;

namespace Repository.Interfaces
{
    public interface IGetUsers
    {
        public List<ExploreEaseUser> GetUsers();
    }
}
