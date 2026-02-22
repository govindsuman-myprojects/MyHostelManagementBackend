using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using Twilio;
using Twilio.Rest.Verify.V2.Service;

namespace MyHostelManagement.Controllers;
[Route("api/otp")]
[ApiController]
[AllowAnonymous]
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
    public async Task<IActionResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
    {
        var accountSid = _config["Twilio:AccountSid"];
        var authToken = _config["Twilio:AuthToken"];
        var serviceSid = _config["Twilio:VerifyServiceSid"];

        TwilioClient.Init(accountSid, authToken);

        var result = await VerificationCheckResource.CreateAsync(
            to: verifyOtpDto.PhoneNumber,
            code: verifyOtpDto.Code,
            pathServiceSid: serviceSid
        );

        if (result.Status == "approved")
            return Ok(new { success = true });

        return Ok(new { success = false });
    }
}
