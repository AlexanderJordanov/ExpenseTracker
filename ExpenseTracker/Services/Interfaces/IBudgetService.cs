using ExpenseTracker.ViewModels.Budgets;

public interface IBudgetService
{
    Task<BudgetIndexViewModel> GetUserBudgetsAsync(string userId);
    Task<BudgetSummaryViewModel> GetCurrentMonthBudgetAsync(string userId);
    Task<IEnumerable<CategoryBudgetInputViewModel>> GetUserCategoryBudgetsAsync(string userId);
    Task UpdateCategoryBudgetAsync(string userId, int categoryId, decimal limit);
}

