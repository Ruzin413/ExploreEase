using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class HotelImage
{
    public int HotelImageId { get; set; }

    [Required]
    public int DayHotelId { get; set; }
    [Required]
    public int DayNumber { get; set; }
    [ForeignKey("DayHotelId")]
    public DayHotel DayHotel { get; set; }

    [Required]
    [StringLength(255)]
    public string ImagePath { get; set; }

}
