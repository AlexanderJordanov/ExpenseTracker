using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Common;

namespace ExpenseTracker.Data.Models
{
    public class UserCategoryBudget
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Range(
            ValidationConstants.Budget.MonthlyLimitMinValue,
            ValidationConstants.Budget.MonthlyLimitMaxValue)]
        public decimal MonthlyLimit { get; set; }
    }
}

