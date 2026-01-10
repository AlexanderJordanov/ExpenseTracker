using ExpenseTracker.ViewModels.Budgets;
using System.Collections.Generic;

namespace ExpenseTracker.ViewModels.Expenses
{
    public class ExpensesIndexViewModel
    {
        public IEnumerable<ExpenseListViewModel> Expenses { get; set; }
            = new List<ExpenseListViewModel>();

        public BudgetSummaryViewModel Budget { get; set; }
            = new BudgetSummaryViewModel();
    }
}
