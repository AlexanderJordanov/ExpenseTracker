using ExpenseTracker.ViewModels.Expenses;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseListViewModel>> GetUserExpensesAsync(string userId);

        Task<ExpenseFormViewModel> GetExpenseFormModelAsync();

        Task CreateExpenseAsync(string userId, ExpenseFormViewModel model);

        Task<ExpenseFormViewModel?> GetExpenseForEditAsync(int id, string userId);

        Task<bool> UpdateExpenseAsync(int id, string userId, ExpenseFormViewModel model);

        Task<bool> SoftDeleteExpenseAsync(int id, string userId);

        Task<ExpenseStatisticsViewModel> GetStatisticsAsync(string userId);
    }
}

