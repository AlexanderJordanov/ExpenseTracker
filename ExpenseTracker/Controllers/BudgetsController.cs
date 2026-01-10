using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    public class BudgetsController : Controller
    {
        private readonly IBudgetService _budgetService;

        public BudgetsController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var model = await _budgetService.GetCurrentMonthBudgetAsync(userId);

            return View(model);
        }


        //HERE
        [Authorize]
        public async Task<IActionResult> Manage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _budgetService.GetUserCategoryBudgetsAsync(userId);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int categoryId, decimal monthlyLimit)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _budgetService.UpdateCategoryBudgetAsync(
                userId, categoryId, monthlyLimit);

            return RedirectToAction(nameof(Manage));
        }

    }
}
