using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int TourPackageId { get; set; }
        [StringLength(1000000)]
        public string Name { get; set; }
        [StringLength(1000000)]
        public string email { get; set; }
        public int PaymentId { get; set; }
        public int Rating { get; set; }

        [StringLength(1000000)]
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
