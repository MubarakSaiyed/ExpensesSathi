namespace ExpensesSathi.Models
{
    public class DebtModel
    {
        // Adding an Id for uniquely identifying each debt (usually used in databases)
        public int Id { get; set; }  // Unique identifier for each debt (primary key)
        
        // Description of the debt, such as the creditor or the loan purpose
        public string Description { get; set; } = string.Empty;

        // Amount of the debt
        public decimal Amount { get; set; }

        // The due date of the debt
        public DateTime DueDate { get; set; }

        // Status can be "Pending" or "Cleared"
        public string Status { get; set; } = "Pending";  

        // Date when the debt is cleared, nullable because it may not be set if the debt is still pending
        public DateTime? ClearedDate { get; set; }

        // Optional notes or additional info about the debt
        public string Notes { get; set; } = string.Empty;

        
    }
}