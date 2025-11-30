using System.Collections.Generic;

namespace ExpenseTracker.ViewModels.Expenses
{
    public class ExpenseStatisticsViewModel
    {
        public IEnumerable<ExpenseCategoryStatisticsViewModel> CategoryStats { get; set; }
            = new List<ExpenseCategoryStatisticsViewModel>();

        public decimal TotalAmount { get; set; }
    }

    public class ExpenseCategoryStatisticsViewModel
    {
        public string CategoryName { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public double Percentage { get; set; }
    }
}

