namespace MyHostelManagement.DTOs
{
    public class NotifcationsResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
