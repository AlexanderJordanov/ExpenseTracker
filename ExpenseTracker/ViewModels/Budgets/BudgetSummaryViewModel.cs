using System.Collections.Generic;

namespace ExpenseTracker.ViewModels.Budgets
{
    public class BudgetSummaryViewModel
    {
        public IList<BudgetCategoryViewModel> Categories { get; set; }
            = new List<BudgetCategoryViewModel>();

        public decimal TotalLimit { get; set; }

        public decimal TotalSpent { get; set; }

        public double OverallUsagePercent { get; set; }
    }
}
