using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using ExpenseTracker.ViewModels.Budgets;
using Microsoft.EntityFrameworkCore;

public class BudgetService : IBudgetService
{
    private readonly ApplicationDbContext _context;

    public BudgetService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BudgetIndexViewModel> GetUserBudgetsAsync(string userId)
    {
        var categories = await _context.Categories
            .Select(c => new
            {
                c.Id,
                c.Name,
                Budget = c.UserCategoryBudgets
                    .FirstOrDefault(b => b.UserId == userId)
            })
            .ToListAsync();

        var model = new BudgetIndexViewModel();

        foreach (var c in categories)
        {
            model.Categories.Add(new BudgetCategoryRowViewModel
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                MonthlyLimit = c.Budget?.MonthlyLimit ?? 0
            });
        }

        return model;
    }

    public async Task<BudgetSummaryViewModel> GetCurrentMonthBudgetAsync(string userId)
    {
        var now = DateTime.UtcNow;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var nextMonthStart = monthStart.AddMonths(1);

        var categories = await _context.Categories
            .Select(c => new
            {
                c.Name,
                Budget = c.UserCategoryBudgets
                    .FirstOrDefault(b => b.UserId == userId),

                Spent = c.Expenses
                    .Where(e =>
                        e.UserId == userId &&
                        e.Date >= monthStart &&
                        e.Date < nextMonthStart &&
                        !e.IsDeleted)
                    .Sum(e => (decimal?)e.Amount) ?? 0m
            })
            .ToListAsync();

        var summary = new BudgetSummaryViewModel();

        foreach (var c in categories)
        {
            var limit = c.Budget?.MonthlyLimit ?? 0m;

            var usagePercent = limit > 0
                ? (double)(c.Spent / limit * 100)
                : 0;

            var status =
                limit == 0 ? "NoLimit" :
                usagePercent >= 100 ? "Exceeded" :
                usagePercent >= 80 ? "Warning" :
                "OK";

            summary.Categories.Add(new BudgetCategoryViewModel
            {
                CategoryName = c.Name,
                MonthlyLimit = limit,
                SpentThisMonth = c.Spent,
                UsagePercent = usagePercent,
                Status = status
            });

            summary.TotalLimit += limit;
            summary.TotalSpent += c.Spent;
        }

        if (summary.TotalLimit > 0)
        {
            summary.OverallUsagePercent =
                (double)(summary.TotalSpent / summary.TotalLimit * 100);
        }

        return summary;
    }
    public async Task<IEnumerable<CategoryBudgetInputViewModel>> GetUserCategoryBudgetsAsync(string userId)
    {
        return await _context.Categories
            .Select(c => new CategoryBudgetInputViewModel
            {
                CategoryId = c.Id,
                CategoryName = c.Name,
                MonthlyLimit = c.UserCategoryBudgets
                    .Where(b => b.UserId == userId)
                    .Select(b => b.MonthlyLimit)
                    .FirstOrDefault()
            })
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }

    public async Task UpdateCategoryBudgetAsync(
    string userId,
    int categoryId,
    decimal limit)
    {
        var budget = await _context.UserCategoryBudgets
            .FirstOrDefaultAsync(b =>
                b.UserId == userId &&
                b.CategoryId == categoryId);

        if (budget == null)
        {
            _context.UserCategoryBudgets.Add(new UserCategoryBudget
            {
                UserId = userId,
                CategoryId = categoryId,
                MonthlyLimit = limit
            });
        }
        else
        {
            budget.MonthlyLimit = limit;
        }

        await _context.SaveChangesAsync();
    }

}

