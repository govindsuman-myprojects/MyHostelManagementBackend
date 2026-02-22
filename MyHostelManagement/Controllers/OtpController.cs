using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace MyHostelManagement.Controllers;
[Route("api/otp")]
[ApiController]
public class OtpController : ControllerBase
{
    private readonly IConfiguration _config;

    public OtpController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
    {
        var accountSid = _config["Twilio:AccountSid"];
        var authToken = _config["Twilio:AuthToken"];
        var serviceSid = _config["Twilio:VerifyServiceSid"];

        TwilioClient.Init(accountSid, authToken);

        await VerificationResource.CreateAsync(
            to: phoneNumber,
            channel: "sms",
            pathServiceSid: serviceSid
        );

        return Ok("OTP Sent");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyOtp(string phoneNumber, string code)
    {
        var accountSid = _config["Twilio:AccountSid"];
        var authToken = _config["Twilio:AuthToken"];
        var serviceSid = _config["Twilio:VerifyServiceSid"];

        TwilioClient.Init(accountSid, authToken);

        var result = await VerificationCheckResource.CreateAsync(
            to: phoneNumber,
            code: code,
            pathServiceSid: serviceSid
        );

        if (result.Status == "approved")
            return Ok(new { success = true });

        return Ok(new { success = false });
    }
}
