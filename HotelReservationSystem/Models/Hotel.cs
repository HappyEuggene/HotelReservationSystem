using HotelReservationSystem.Models;

public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RoomCount { get; set; }
    public bool VacationHotel { get; set; }
    public string Description { get; set; }
    public string Contact { get; set; }
    public double? Rating { get; set; }
    public int? AddressId { get; set; }
    public Address? Address { get; set; }
    public ICollection<Room>? Rooms { get; set; } = new List<Room>();
    public ICollection<HotelReservation>? HotelReservations { get; set; } = new List<HotelReservation>();
    public ICollection<Review>? Reviews { get; set; } = new List<Review>();
    public string? PictureUrl { get; set; }
    public double? AverageRating => Reviews.Any() ? (double?)Reviews.Average(r => r.Rating) : null;
}

