using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels;

public class MyProfileViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50)]
    public string LastName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}
