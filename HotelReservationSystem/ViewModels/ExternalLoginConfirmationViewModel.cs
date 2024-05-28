using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels;

public class ExternalLoginConfirmationViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

}
