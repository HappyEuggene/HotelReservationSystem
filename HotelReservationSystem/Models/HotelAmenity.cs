namespace HotelReservationSystem.Models
{
    public class HotelAmenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();
    }

}
