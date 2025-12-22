namespace MyHostelManagement.Api.DTOs
{
    public class LoginDto
    {
        public string EmailOrPhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserRoleData { get; set; } = string.Empty;
    }
}
