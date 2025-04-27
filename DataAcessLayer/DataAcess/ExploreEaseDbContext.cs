using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.DataAcess
{
    public class ExploreEaseDbContext : IdentityDbContext<ExploreEaseUser> , IExploreEaseUserDbContext
    {
        public ExploreEaseDbContext(DbContextOptions<ExploreEaseDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public void Configure(EntityTypeBuilder<ExploreEaseUser> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
        }
        public DbSet<ExploreEaseUser> ExploreEaseUser { get; set; }
        public DbSet<TourPackage> TourPackage { get; set; }
        public DbSet<DayHotel> dayHotels { get; set; }
        public  DbSet<HotelImage> hotelImage { get; set; }
        
    }
}
