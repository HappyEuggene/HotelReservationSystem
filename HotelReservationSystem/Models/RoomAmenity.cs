namespace HotelReservationSystem.Models
{
    public class RoomAmenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? AmenityId { get; set; }
        public Room? HotelRoom { get; set; }

    }
}
