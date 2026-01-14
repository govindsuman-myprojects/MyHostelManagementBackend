namespace MyHostelManagement.DTOs
{
    public class ComplaintResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid CategoryId { get; set; }
        public int Status { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
