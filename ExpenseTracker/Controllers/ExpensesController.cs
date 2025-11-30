using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.ViewModels.Expenses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var expenses = await _expenseService.GetUserExpensesAsync(userId);

            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _expenseService.GetExpenseFormModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpenseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var filledModel = await _expenseService.GetExpenseFormModelAsync();
                model.Categories = filledModel.Categories;

                return View(model);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await _expenseService.CreateExpenseAsync(userId, model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var model = await _expenseService.GetExpenseForEditAsync(id, userId);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpenseFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var filledModel = await _expenseService.GetExpenseFormModelAsync();
                model.Categories = filledModel.Categories;

                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var success = await _expenseService.UpdateExpenseAsync(id, userId, model);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var success = await _expenseService.SoftDeleteExpenseAsync(id, userId);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Statistics()
        {
            // тук после ще правим статистиките
            return View();
        }
    }
}
