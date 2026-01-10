using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.ViewModels.Budgets;
using ExpenseTracker.ViewModels.Expenses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ExpenseListViewModel>> GetUserExpensesAsync(string userId)
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            return await _context.Expenses
                .Where(e => e.UserId == userId && !e.IsDeleted)
                .OrderByDescending(e => e.Date)
                .Select(e => new ExpenseListViewModel
                {
                    Id = e.Id,
                    CategoryName = e.Category.Name,
                    Date = e.Date,
                    Amount = e.Amount,
                    Description = e.Description,

                    BudgetStatus = _context.UserCategoryBudgets
                        .Where(b => b.UserId == userId && b.CategoryId == e.CategoryId)
                        .Select(b =>
                            b.MonthlyLimit == 0 ? null :
                            (
                                _context.Expenses
                                    .Where(x =>
                                        x.UserId == userId &&
                                        x.CategoryId == e.CategoryId &&
                                        !x.IsDeleted &&
                                        x.Date >= monthStart &&
                                        x.Date < nextMonthStart)
                                    .Sum(x => x.Amount)
                                >= b.MonthlyLimit
                                    ? "Exceeded"
                                    :
                                _context.Expenses
                                    .Where(x =>
                                        x.UserId == userId &&
                                        x.CategoryId == e.CategoryId &&
                                        !x.IsDeleted &&
                                        x.Date >= monthStart &&
                                        x.Date < nextMonthStart)
                                    .Sum(x => x.Amount)
                                >= b.MonthlyLimit * 0.8m
                                    ? "Warning"
                                    : "OK"
                            )
                        )
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<ExpenseFormViewModel> GetExpenseFormModelAsync()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return new ExpenseFormViewModel
            {
                Categories = categories
            };
        }
        public async Task CreateExpenseAsync(string userId, ExpenseFormViewModel model)
        {
            var expense = new Expense
            {
                Amount = model.Amount,
                Date = model.Date,
                Description = model.Description,
                CategoryId = model.CategoryId,
                UserId = userId
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }
        public async Task<ExpenseFormViewModel?> GetExpenseForEditAsync(int id, string userId)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return null;
            }

            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync();

            return new ExpenseFormViewModel
            {
                Id = expense.Id,
                Amount = expense.Amount,
                Date = expense.Date,
                Description = expense.Description,
                CategoryId = expense.CategoryId,
                Categories = categories
            };
        }
        public async Task<bool> UpdateExpenseAsync(int id, string userId, ExpenseFormViewModel model)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return false;
            }

            expense.Amount = model.Amount;
            expense.Date = model.Date;
            expense.Description = model.Description;
            expense.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> SoftDeleteExpenseAsync(int id, string userId)
        {
            var expense = await _context.Expenses
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return false;
            }

            expense.IsDeleted = true;
            expense.DeletedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ExpenseStatisticsViewModel> GetStatisticsAsync(string userId)
        {
            var grouped = await _context.Expenses
                .Where(e => e.UserId == userId)
                .GroupBy(e => e.Category.Name)
                .Select(g => new
                {
                    CategoryName = g.Key,
                    TotalAmount = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            if (!grouped.Any())
            {
                return new ExpenseStatisticsViewModel();
            }

            var total = grouped.Sum(x => x.TotalAmount);

            var categoryStats = grouped
                .Select(x => new ExpenseCategoryStatisticsViewModel
                {
                    CategoryName = x.CategoryName,
                    TotalAmount = x.TotalAmount,
                    Percentage = (double)(x.TotalAmount / total * 100)
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToList();

            return new ExpenseStatisticsViewModel
            {
                CategoryStats = categoryStats,
                TotalAmount = total
            };
        }

        public async Task<BudgetSummaryViewModel> GetCurrentMonthBudgetAsync(string userId)
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1);
            var nextMonthStart = monthStart.AddMonths(1);

            var data = await _context.UserCategoryBudgets
                .Where(b => b.UserId == userId)
                .Select(b => new
                {
                    CategoryName = b.Category.Name,
                    MonthlyLimit = b.MonthlyLimit,

                    Spent = _context.Expenses
                        .Where(e =>
                            e.UserId == userId &&
                            e.CategoryId == b.CategoryId &&
                            e.Date >= monthStart &&
                            e.Date < nextMonthStart &&
                            !e.IsDeleted)
                        .Sum(e => (decimal?)e.Amount) ?? 0m
                })
                .ToListAsync();

            var summary = new BudgetSummaryViewModel();

            foreach (var item in data)
            {
                var usagePercent = item.MonthlyLimit > 0
                    ? (double)(item.Spent / item.MonthlyLimit * 100)
                    : 0;

                string status;
                if (item.MonthlyLimit == 0)
                {
                    status = "NoLimit";
                }
                else if (usagePercent >= 100)
                {
                    status = "Exceeded";
                }
                else if (usagePercent >= 80)
                {
                    status = "Warning";
                }
                else
                {
                    status = "OK";
                }

                summary.Categories.Add(new BudgetCategoryViewModel
                {
                    CategoryName = item.CategoryName,
                    MonthlyLimit = item.MonthlyLimit,
                    SpentThisMonth = item.Spent,
                    UsagePercent = usagePercent,
                    Status = status
                });

                summary.TotalLimit += item.MonthlyLimit;
                summary.TotalSpent += item.Spent;
            }

            if (summary.TotalLimit > 0)
            {
                summary.OverallUsagePercent =
                    (double)(summary.TotalSpent / summary.TotalLimit * 100);
            }

            return summary;
        }


    }
}
