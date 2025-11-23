namespace MyHostelManagement.Api.DTOs
{
    public class MaintenanceDto
    {
        public int Id { get; set; }
        public int HostelId { get; set; }
        public string Issue { get; set; } = string.Empty;
        public DateTime ReportedOn { get; set; }
        public bool IsResolved { get; set; }
    }
}
