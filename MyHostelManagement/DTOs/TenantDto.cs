namespace MyHostelManagement.Api.DTOs
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Guid BedId { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
    }
}
