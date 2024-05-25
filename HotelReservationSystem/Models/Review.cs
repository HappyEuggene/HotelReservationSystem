using HotelReservationSystem.Models;

public class Review
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string EntityType { get; set; }
    public int EntityId { get; set; } 
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime Date { get; set; }

    public User User { get; set; }
    public Hotel Hotel { get; set; }
}
