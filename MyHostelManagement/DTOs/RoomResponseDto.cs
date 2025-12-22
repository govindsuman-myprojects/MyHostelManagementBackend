namespace MyHostelManagement.Api.DTOs
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Rent { get; set; }
        public char RoomFloor { get; set; }
        public List<BedResponseDto> Beds { get; set; }
    }
}
