namespace MyHostelManagement.Api.DTOs
{
    public class BedDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string BedNumber { get; set; } = string.Empty;
        public bool IsOccupied { get; set; }
    }
}
