using System.ComponentModel.DataAnnotations;

namespace HotelReservationSystem.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Старий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новий пароль")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароль і підтвердження пароля не збігаються.")]
        [Display(Name = "Підтвердити новий пароль")]
        public string ConfirmPassword { get; set; }
    }
}
