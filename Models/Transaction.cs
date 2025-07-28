namespace ExpensesSathi.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty;  // Type: credit, debit
        public string Tags { get; set; } = string.Empty;  // Tags
        public string Notes { get; set; } = string.Empty; // Notes
        public string Status { get; set; } = string.Empty; // Status: Cleared, Pending
    }
}