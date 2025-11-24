using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data.Seeding
{
    public class ApplicationDbContextSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Database.CanConnect())
            {
                await dbContext.Database.MigrateAsync();
            }

            await new RolesSeeder().SeedAsync(dbContext, serviceProvider);
            await new AdminUserSeeder().SeedAsync(dbContext, serviceProvider);
            await new CategoriesSeeder().SeedAsync(dbContext, serviceProvider);
        }
    }
}
