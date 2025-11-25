using ExpenseTracker.ViewModels.Expenses;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseListViewModel>> GetUserExpensesAsync(string userId);

        Task<ExpenseFormViewModel> GetExpenseFormModelAsync();

        Task CreateExpenseAsync(string userId, ExpenseFormViewModel model);
    }
}

