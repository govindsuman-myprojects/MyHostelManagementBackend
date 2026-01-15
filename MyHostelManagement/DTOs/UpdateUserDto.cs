namespace MyHostelManagement.DTOs
{
    public class UpdateUserDto
    {
        public string? Name { get; set; }
        public decimal? RentAmount { get; set; }
        public int RentCycle { get; set; }
        public decimal? AdvanceAmount { get; set; }

        public string PhoneNumber { get; set; }
        public string? GurdianName { get; set; }
        public long? GurdianPhoneNumber { get; set; }

        public int Status { get; set; }
    }

}
