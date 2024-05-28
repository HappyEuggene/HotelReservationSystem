using HotelReservationSystem.Models;

public class Reservation
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public int? HotelId { get; set; }
    public int? RoomId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalPrice { get; set; }
    public string Status { get; set; }

    public User? User { get; set; }
    public ICollection<HotelReservation>? HotelReservations { get; set; } = new List<HotelReservation>();
    public Room? Room { get; set; }
}
