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
    }
}
