using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNuber { get; set; }

        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}
