using AtmMonitor.Core.Enums;

namespace AtmMonitor.Core.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime OperationDate { get; set; }
        public int ATMId { get; set; }
        public TransactionType Type { get; set; } 
        public decimal Amount { get; set; }
        public ATM ATM { get; set; }
    }
}
