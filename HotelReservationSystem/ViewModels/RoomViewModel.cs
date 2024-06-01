namespace HotelReservationSystem.ViewModels;

public class RoomViewModel
{
    public int Id { get; set; }  // Добавьте идентификатор, чтобы можно было обновлять существующие записи
    public int RoomNumber { get; set; }
    public double PricePerNight { get; set; }
    public bool IsAvailable { get; set; }
    public double? Rating { get; set; }

    public List<RoomAmenityViewModel>? RoomAmenities { get; set; } = new List<RoomAmenityViewModel>();
}
    