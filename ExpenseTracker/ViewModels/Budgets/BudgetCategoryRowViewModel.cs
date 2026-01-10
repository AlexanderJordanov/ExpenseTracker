using System.ComponentModel.DataAnnotations;

public class BudgetCategoryRowViewModel
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    [Range(0, 100000)]
    public decimal MonthlyLimit { get; set; }
}

