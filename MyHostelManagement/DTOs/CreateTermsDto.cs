namespace MyHostelManagement.DTOs
{
    public class CreateTermsDto
    {
        public Guid HostelId { get; set; }
        public Guid RoleId { get; set; }
        public string? Content { get; set; }
    }

}
