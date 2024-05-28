using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels;

public class ReviewViewModel
{
    public int HotelId { get; set; }

    [Range(1, 5)]
    public double Rating { get; set; }

    [Required]
    [StringLength(500)]
    public string Comment { get; set; }
}
