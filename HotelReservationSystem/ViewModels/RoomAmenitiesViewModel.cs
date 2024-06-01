using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.ViewModels
{
    public class RoomAmenitiesViewModel
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public bool IsAvailable { get; set; }
        public int Rating { get; set; }

        public List<AmenityViewModel> Amenities { get; set; }
    }

    public class AmenityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AmenityId { get; set; }
        public int HotelRoomId { get; set; }
    }

}
