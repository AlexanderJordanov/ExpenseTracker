using ExpenseTracker.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ExpenseTracker.Common.ValidationConstants;

namespace ExpenseTracker.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.Category.NameMaxLength)]
        public string Name { get; set; } = null!;

        [Range(ValidationConstants.Category.MonthlyLimitMinValue, ValidationConstants.Category.MonthlyLimitMaxValue)]
        public decimal MonthlyLimit { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    }
}
