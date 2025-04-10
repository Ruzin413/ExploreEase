using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DataAcess
{
    public interface IExploreEaseUserDbContext
    {
        public DbSet<ExploreEaseUser> ExploreEaseUser { get; set; }
    }
}
