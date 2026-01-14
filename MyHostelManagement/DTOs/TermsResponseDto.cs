namespace MyHostelManagement.DTOs
{
    public class TermsResponseDto
    {
        public Guid Id { get; set; }
        public Guid HostelId { get; set; }
        public Guid RoleId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
