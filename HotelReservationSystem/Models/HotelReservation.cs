namespace HotelReservationSystem.Models;

public class HotelReservation
{
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; }

    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
}
