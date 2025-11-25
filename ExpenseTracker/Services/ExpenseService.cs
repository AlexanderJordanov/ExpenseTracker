using ExpenseTracker.Data;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.ViewModels.Expenses;
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
    }
}
