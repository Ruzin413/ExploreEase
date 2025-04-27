using Models.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class DayHotel
{
    public int DayHotelId { get; set; }

    [Required]
    public int TourPackageId { get; set; }

    [ForeignKey("TourPackageId")]
    public TourPackage TourPackage { get; set; }

    [Range(1, 30)]
    public int DayNumber { get; set; }

    [Required]
    [StringLength(100)]
    public string HotelName { get; set; }

    [StringLength(1000)]
    public string HotelDescription { get; set; }

    [StringLength(200)]
    public string HotelLocation { get; set; }

    public ICollection<HotelImage> HotelImages { get; set; }
}
