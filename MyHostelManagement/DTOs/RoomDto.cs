namespace MyHostelManagement.Api.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public int HostelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
    }
}
