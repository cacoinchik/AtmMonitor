namespace AtmMonitor.API.DTOs
{
    public class TransactionFilterRequest
    {
        public List<int>? ATMIds { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
