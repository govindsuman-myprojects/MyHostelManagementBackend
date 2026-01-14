namespace MyHostelManagement.DTOs
{
    public class UpdateRoomDto
    {
        public string RoomNumber { get; set; }
        public int TotalBeds { get; set; }
        public decimal Rent { get; set; }
        public int Type { get; set; }
    }

}
