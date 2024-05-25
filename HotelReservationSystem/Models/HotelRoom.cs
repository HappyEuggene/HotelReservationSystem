using HotelReservationSystem.Models;

public class HotelRoom
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string RoomCount { get; set; } 
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Rating { get; set; }

    public Hotel Hotel { get; set; } // Навігаційна властивість
    public ICollection<RoomAmenity>? RoomAmenities { get; set; } = new List<RoomAmenity>();
}

