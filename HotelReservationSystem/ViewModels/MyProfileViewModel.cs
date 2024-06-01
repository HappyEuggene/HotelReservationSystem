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

    public string? BirthDate { get; set; }

    public string? Nationality { get; set; }

    public string? Gender { get; set; }

    public string? ResidentialAddress { get; set; }

}
