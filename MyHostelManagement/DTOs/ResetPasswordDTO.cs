namespace MyHostelManagement.Api.DTOs
{
    public class ResetPasswordDTO
    {
        public string EmailOrPhone { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
