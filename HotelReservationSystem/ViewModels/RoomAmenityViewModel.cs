namespace HotelReservationSystem.ViewModels
{
    public class RoomAmenityViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int AmenityId { get; set; }
        public int HotelRoomId { get; set; }
    }
}
