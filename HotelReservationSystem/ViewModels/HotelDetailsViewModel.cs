using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.ViewModels
{
    public class HotelDetailsViewModel : Controller
    {
        public Hotel Hotel { get; set; }
        public List<RoomAmenitiesViewModel> Rooms { get; set; }
    }
}
