namespace HotelReservationSystem.Models
{
    public class RoomAmenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Room>? HotelRooms { get; set; } = new List<Room>();
    }
}
