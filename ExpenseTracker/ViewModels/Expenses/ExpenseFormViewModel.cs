using System;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseTracker.ViewModels.Expenses
{
    public class ExpenseFormViewModel
    {
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();

        [Required]
        [Range(ValidationConstants.Expense.AmountMinValue, ValidationConstants.Expense.AmountMaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [MaxLength(ValidationConstants.Expense.DescriptionMaxLength)]
        public string? Description { get; set; }
    }
}

