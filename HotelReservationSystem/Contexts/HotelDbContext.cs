using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelReservationSystem.Models;

namespace HotelReservationSystem.Contexts
{
    public class HotelDbContext : IdentityDbContext<User>
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Room>()
                .HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HotelReservation>()
                .HasKey(hr => new { hr.HotelId, hr.ReservationId });

            builder.Entity<HotelReservation>()
                .HasOne(hr => hr.Hotel)
                .WithMany(h => h.HotelReservations)
                .HasForeignKey(hr => hr.HotelId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<HotelReservation>()
                .HasOne(hr => hr.Reservation)
                .WithMany(r => r.HotelReservations)
                .HasForeignKey(hr => hr.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<HotelAmenity> HotelAmenities { get; set; }
        public DbSet<RoomAmenity> RoomAmenities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<HotelReservation> HotelReservations { get; set; }
    }
}
