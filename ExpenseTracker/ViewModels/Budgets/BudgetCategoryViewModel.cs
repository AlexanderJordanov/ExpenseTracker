namespace ExpenseTracker.ViewModels.Budgets
{
    public class BudgetCategoryViewModel
    {
        public string CategoryName { get; set; } = null!;

        public decimal MonthlyLimit { get; set; }

        public decimal SpentThisMonth { get; set; }

        public double UsagePercent { get; set; }

        // "OK", "Warning", "Exceeded"
        public string Status { get; set; } = null!;

        public string ProgressBarClass =>
        Status switch
        {
            "Exceeded" => "bg-danger",
            "Warning" => "bg-warning",
            _ => "bg-success"
        };

        public string BadgeClass =>
            Status switch
            {
                "Exceeded" => "bg-danger",
                "Warning" => "bg-warning text-dark",
                _ => "bg-success"
            };
    }
}

