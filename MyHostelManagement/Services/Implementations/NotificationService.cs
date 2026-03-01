using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Interfaces;
using System;

namespace MyHostelManagement.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotification(Guid hostelId, Guid userId,
            string title, string message, string type)
        {
            var notification = new Notification
            {
                HostelId = hostelId,
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotifcationsResponseDto>> GetNotifications(Guid hostelId)
        {
            var response = await _context.Notifications
                           .Where(x => x.HostelId == hostelId && !x.IsRead)
                           .OrderByDescending(x => x.CreatedAt)
                           .ToListAsync();

            return response.Select(Map).ToList();
        }

        public async Task<NotifcationsResponseDto> MarkAsReadAsync(Guid userId)
        {
            var notification = await _context.Notifications
                           .Where(x => x.Id == userId)
                           .FirstOrDefaultAsync();

            if (notification == null) return new NotifcationsResponseDto();

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return Map(notification);
        }

        private static NotifcationsResponseDto Map(Notification notification)
        {
            return new NotifcationsResponseDto
            {
                Id = notification.Id,
                HostelId = notification.HostelId,
                IsRead = notification.IsRead,
                Message = notification.Message,
                Title = notification.Title,
                CreatedAt = notification.CreatedAt,
                Type = notification.Type,
                UserId = notification.UserId
            };
        }
    }
}
