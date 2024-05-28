using HotelReservationSystem.Models;

namespace HotelReservationSystem.ViewModels;

public class HotelViewModel
{
    public Hotel Hotel { get; set; }
    public Address Address { get; set; }
    public List<RoomViewModel>? Rooms { get; set; }
    public BulkAddRoomsViewModel? BulkAddRooms { get; set; }
    public IFormFile? Picture { get; set; }
}
