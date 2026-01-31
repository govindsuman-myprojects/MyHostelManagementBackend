namespace MyHostelManagement.DTOs
{
    public class CreateUserDto
    {
        public Guid HostelId { get; set; }
        public Guid? RoomId { get; set; }

        public string? Name { get; set; }
        public decimal? RentAmount { get; set; }
        public int RentCycle { get; set; }
        public decimal? AdvanceAmount { get; set; }

        public string PhoneNumber { get; set; }
        public string? GurdianName { get; set; }
        public long? GurdianPhoneNumber { get; set; }

        public string? AadharCardNumber { get; set; }
        public string? AadharCardFile { get; set; }

        public string Password { get; set; } = string.Empty;
    }

}
