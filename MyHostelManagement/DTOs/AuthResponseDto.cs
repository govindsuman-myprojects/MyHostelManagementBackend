namespace MyHostelManagement.DTOs
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string Role { get; set; }
        public Guid UserId { get; set; }
        public Guid? HostelId { get; set; }
    }
}
