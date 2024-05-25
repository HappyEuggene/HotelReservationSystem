public class Hotel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; // Додайте ініціалізацію
    public string Address { get; set; } = string.Empty; // Додайте ініціалізацію
    public bool VacationHotel { get; set; }
    public string Description { get; set; } = string.Empty; // Додайте ініціалізацію
    public string Contact { get; set; } = string.Empty; // Додайте ініціалізацію
    public decimal Rating { get; set; }

    public ICollection<HotelRoom>? HotelRooms { get; set; } = new List<HotelRoom>();
    public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    public ICollection<Review>? Reviews { get; set; } = new List<Review>();
}
