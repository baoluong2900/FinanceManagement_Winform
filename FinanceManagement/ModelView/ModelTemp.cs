namespace FinanceManagement.ModelTemp
{
    public class ModelTemp
    {
        public class ValueView
        {
            public string Text { get; set; } = string.Empty;

            public string Message { get; set; } = string.Empty;

            public ValueView(string text, string message)
            {
                Text = text;
                Message = message;
            }
        }



        public const string DebtName = "Nợ";
        public const string LoanName = "Vay mượn";
    }
}