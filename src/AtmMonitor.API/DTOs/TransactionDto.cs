namespace AtmMonitor.API.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public DateTime OperationDate { get; set; }
        public int ATMId { get; set; }
        public string ATMAddress { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
