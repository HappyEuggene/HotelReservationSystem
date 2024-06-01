using Microsoft.AspNetCore.Mvc;

namespace HotelReservationSystem.ViewModels
{
    public class AmenityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AmenityId { get; set; }
        public int HotelRoomId { get; set; }
    }
}
}
