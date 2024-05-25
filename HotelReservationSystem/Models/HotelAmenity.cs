namespace HotelReservationSystem.Models
{
    public class HotelAmenity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Додайте ініціалізацію
        public string Description { get; set; } = string.Empty; // Додайте ініціалізацію

        public ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();
    }

}
