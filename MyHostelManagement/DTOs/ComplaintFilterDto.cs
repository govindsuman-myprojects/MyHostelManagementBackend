namespace MyHostelManagement.DTOs
{
    public class ComplaintFilterDto
    {
        public Guid HostelId { get; set; }
        public int? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? UserId { get; set; }
    }
}
