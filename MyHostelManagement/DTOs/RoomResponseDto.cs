namespace MyHostelManagement.Api.DTOs
{
    public class RoomResponseDto
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public int TotalBeds { get; set; }
        public int OccupiedBeds { get; set; }
        public decimal Rent { get; set; }
        public int Type { get; set; }
    }


}
