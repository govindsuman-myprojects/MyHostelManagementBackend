namespace MyHostelManagement.DTOs
{
    public class RoomResponseUIDto
    {
        public char Floor { get; set; }
        public List<RoomSummaryDto> Rooms { get; set; } = new();
    }

    public class RoomSummaryDto
    {
        public Guid RoomId { get; set; }
        public string RoomNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int OccupiedBeds { get; set; }
        public int VacantBeds { get; set; }
        public string Type { get; set; } = string.Empty;
        public int Rent { get; set; }
    }
}
