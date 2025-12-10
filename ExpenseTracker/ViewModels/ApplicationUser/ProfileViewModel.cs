using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Common;

namespace ExpenseTracker.ViewModels.ApplicationUser
{
    public class ProfileViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.User.FirstNameMaxLength)]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.User.LastNameMaxLength)]
        [Display(Name = "Last name")]
        public string LastName { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(ValidationConstants.User.ProfilePictureUrlMaxLength)]
        [Display(Name = "Profile picture URL")]
        public string? ProfilePictureUrl { get; set; }
    }
}
