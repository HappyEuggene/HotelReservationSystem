namespace HotelReservationSystem.ViewModels;

public class RoomViewModel
{
    public int Id { get; set; }  // Добавьте идентификатор, чтобы можно было обновлять существующие записи
    public int RoomNumber { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Rating { get; set; }
}
