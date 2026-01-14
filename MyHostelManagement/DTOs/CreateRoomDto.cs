namespace MyHostelManagement.DTOs
{
    public class CreateRoomDto
    {
        public Guid HostelId { get; set; }
        public string? RoomNumber { get; set; }
        public int TotalBeds { get; set; }
        public decimal Rent { get; set; }
        public int Type { get; set; }
    }

}
