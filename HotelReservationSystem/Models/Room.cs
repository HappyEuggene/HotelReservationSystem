using HotelReservationSystem.Models;

public class Room
{
    public int Id { get; set; }
    public double PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public double? Rating { get; set; }
    public int RoomNumber {  get; set; }
    public int? HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public string? PictureUrl {  get; set; }
    public ICollection<RoomAmenity>? RoomAmenities { get; set; } = new List<RoomAmenity>();
}

