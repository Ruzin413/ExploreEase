using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Models.Models
{
    public class TourPackage
    {
        public int TourPackageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        [Required]
        [StringLength(100)]
        public int Rating { get; set; } 
        public ICollection<DayHotel> DayHotels { get; set; }
    }

}
