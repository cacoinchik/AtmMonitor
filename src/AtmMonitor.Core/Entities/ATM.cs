namespace AtmMonitor.Core.Entities
{
    public class ATM
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime InstallationDate { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
