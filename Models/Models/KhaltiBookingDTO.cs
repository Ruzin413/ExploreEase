using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class KhaltiBookingDTO
    {
        public int TourPackageId { get; set; }
        public decimal Price { get; set; } // Ensure this is decimal or double
        public string PackageName { get; set; }
        public string Username { get; set; } // Add the username field
    }
}
