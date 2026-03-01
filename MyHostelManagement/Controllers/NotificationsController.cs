using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{hostelId}")]
        public async Task<IActionResult> GetNotifications(Guid hostelId)
        {
            var notifications = await _notificationService.GetNotifications(hostelId);
            return Ok(notifications);
        }

        [HttpPut("mark-read/{id}")]
        public async Task<IActionResult> MarkAsRead(Guid id)
        {
            var notification = await _notificationService.MarkAsReadAsync(id);
            return Ok(notification);
        }
    }
}
