using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotification(Guid hostelId, Guid userId, string title, string message, string type);
        Task<List<NotifcationsResponseDto>> GetNotifications(Guid hostelId);
        Task<NotifcationsResponseDto> MarkAsReadAsync(Guid userId);
    }
}
