using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
        public class PaymentModel
        {
            [Required]
            public int id { get; set; }
            [Required]
            public string username { get; set; }
            [Required]
            public int Tourpackageid { get; set; }
            [Required]
            public int price { get; set; }
            [Required]
            public float Rating { get; set; }
            [Required]
            public DateOnly StartDate { get; set; }
            [Required]
            public DateOnly EndDate { get; set; }
            [Required]
            public DateTime BookingDate { get; set; }
            [Required]
            public float Latitude { get; set; }
            [Required]
            public float Longitude { get; set; }
        [Required]
        public int NumberOfPeople { get; set; }
        [Required]
        public int TotalPrice { get; set; }
    }
}

