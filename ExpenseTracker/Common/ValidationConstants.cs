namespace ExpenseTracker.Common
{
    public class ValidationConstants
    {
        public static class User
        {
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
            public const int PreferredCurrencyMaxLength = 10;
            public const int ProfilePictureUrlMaxLength = 2048;
        }

        public static class Category
        {
            public const int NameMaxLength = 50;
            public const int MonthlyLimitMinValue = 0;
            public const int MonthlyLimitMaxValue = 1000000;
        }

        public static class Expense
        {
            public const int DescriptionMaxLength = 200;
            public const double AmountMinValue = 0.01;
            public const int AmountMaxValue = 1000000;
        }
    }
}
