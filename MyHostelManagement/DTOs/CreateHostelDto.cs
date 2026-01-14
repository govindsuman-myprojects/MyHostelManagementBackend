namespace MyHostelManagement.Api.DTOs
{
    public class CreateHostelDto
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? OwnerName { get; set; }
        public long PhoneNumber { get; set; }
    }
}
