using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using ExpenseTracker.Services.Interfaces;
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
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .Select(e => new ExpenseListViewModel
                {
                    Id = e.Id,
                    CategoryName = e.Category.Name,
                    Date = e.Date,
                    Amount = e.Amount,
                    Description = e.Description
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
    }
}
