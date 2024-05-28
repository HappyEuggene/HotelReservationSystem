using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels;

public class ReservationViewModel
{
    public int HotelId { get; set; }
    public int RoomId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
}
