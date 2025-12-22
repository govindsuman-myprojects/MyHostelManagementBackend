namespace MyHostelManagement.Api.DTOs
{
    public class BedResponseDto
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid HostelId { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
