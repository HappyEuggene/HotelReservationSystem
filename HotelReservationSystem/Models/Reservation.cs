using HotelReservationSystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string ReservationType { get; set; } 
    public int EntityId { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    public User User { get; set; }
    public ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();
}
