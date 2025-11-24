using ExpenseTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Data.Seeding
{
    public class CategoriesSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Categories.Any())
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var admin = await userManager.FindByEmailAsync("admin@expensetracker.com");

                var categories = new[]
                {
                    "Food", "Transport", "Utilities", "Entertainment",
                    "Shopping", "Healthcare", "Education", "Other"
                };

                foreach (var cat in categories)
                {
                    dbContext.Categories.Add(new Category
                    {
                        Name = cat,
                        UserId = admin!.Id,
                        MonthlyLimit = 0
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}

