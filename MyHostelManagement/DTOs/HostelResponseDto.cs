namespace MyHostelManagement.DTOs
{
    public class HostelResponseDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? OwnerName { get; set; }
        public long PhoneNumber { get; set; }
    }

}
