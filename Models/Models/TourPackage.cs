﻿using System;
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
        [StringLength(100000)]
        public string Name { get; set; }
        [Required]
        [StringLength(1000000)]
        public string Description { get; set; }
        [Required]
        public float Rating { get; set; } 
        [Required]
        public float price { get; set; }
        [Required]
        public float Lat { get; set; }
        [Required]
        public float Long { get; set; }
        [Required]
        [StringLength(100000)]
        public  string Destination {  get; set; }
        [Required]
        [StringLength(100)]
        public string DestinationImage { get; set; }
        [Required]
        public int NumberOfDays {  get; set; }
        public ICollection<DayHotel> DayHotels { get; set; }
    }
}
