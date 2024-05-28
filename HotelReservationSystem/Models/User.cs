using Microsoft.AspNetCore.Identity;

namespace HotelReservationSystem.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? BirthDate { get; set; }
        public string? Nationality { get; set; }
        public string? Gender { get; set; }
        public string? ResidentialAddress { get; set; }

        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
        public ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    }
}
