namespace MyHostelManagement.Api.DTOs
{
    public class ForgotPasswordDTO
    {
        public string EmailOrPassword { get; set; } = string.Empty;
        public int OTP { get; set; }
        public string NewPassword { get; set; } = string.Empty;
    }
}
