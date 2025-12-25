namespace MyHostelManagement.Api.DTOs
{
    public class TenantDto
    {
        public Guid HostelId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid BedId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? GurdianName { get; set; }
        public string? GurdianPhone {  get; set; }
        public string? Aadhar {  get; set; }
        public string? Address { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? Rent { get; set; }
        public decimal? Advance { get; set; }
        public string Status { get; set; } = "active";
    }
}
