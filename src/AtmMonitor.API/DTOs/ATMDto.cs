namespace AtmMonitor.API.DTOs
{
    public class ATMDto
    {
        public int Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime InstallationDate { get; set; }
    }
}
