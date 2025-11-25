namespace ExpenseTracker.ViewModels.Expenses
{
    public class ExpenseListViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
