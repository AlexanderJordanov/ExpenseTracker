using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Common;

namespace ExpenseTracker.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(ValidationConstants.User.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.User.LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(ValidationConstants.User.PreferredCurrencyMaxLength)]
        public string? PreferredCurrency { get; set; }

        [MaxLength(ValidationConstants.User.ProfilePictureUrlMaxLength)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public ICollection<UserCategoryBudget> CategoryBudgets { get; set; }
        = new List<UserCategoryBudget>();
    }
}
