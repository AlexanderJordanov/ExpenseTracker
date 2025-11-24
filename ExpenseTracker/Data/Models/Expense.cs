using System;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Common;

namespace ExpenseTracker.Data.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Range(ValidationConstants.Expense.AmountMinValue, ValidationConstants.Expense.AmountMaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [MaxLength(ValidationConstants.Expense.DescriptionMaxLength)]
        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}

