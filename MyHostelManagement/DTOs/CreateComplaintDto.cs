namespace MyHostelManagement.DTOs
{
    public class CreateComplaintDto
    {
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid RoomId { get; set; }
        public string? Content { get; set; }
    }

}
