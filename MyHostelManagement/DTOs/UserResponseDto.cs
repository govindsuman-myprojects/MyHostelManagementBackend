namespace MyHostelManagement.DTOs
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? RoleName { get; set; }

        public decimal? RentAmount { get; set; }
        public int RentCycle { get; set; }

        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public Guid HostelId { get; set; }
        public Guid? RoomId { get; set; }
        public DateTime? JoiningDate { get; set; }
    }

}
