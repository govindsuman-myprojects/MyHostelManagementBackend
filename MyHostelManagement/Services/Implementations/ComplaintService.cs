using AutoMapper;
using MyHostelManagement.Api.Models;
using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class ComplaintService : IComplaintService
    {
        private readonly IMapper _mapper;
        private readonly IComplaintRepository _complaintRepo;
        private readonly INotificationService _notificationService;
        private readonly IUserRepository _userRepo;
        private readonly IComplaintCategoryRepository _complaintCategoryRepo;

        public ComplaintService(IMapper mapper, IComplaintRepository complaintRepo, INotificationService notificationService,
            IUserRepository userRepo, IComplaintCategoryRepository complaintCategoryRepo)
        {
            _mapper = mapper;
            _complaintRepo = complaintRepo;
            _notificationService = notificationService;
            _userRepo = userRepo;
            _complaintCategoryRepo = complaintCategoryRepo;
        }

        public async Task<ComplaintResponseDto> CreateAsync(CreateComplaintDto dto)
        {
            var complaint = new Complaint
            {
                HostelId = dto.HostelId,
                UserId = dto.UserId,
                RoomId = dto.RoomId,
                CategoryId = dto.CategoryId,
                Content = dto.Content,
                Status = 1 // Active
            };

            await _complaintRepo.CreateAsync(complaint);

            var user = await _userRepo.GetByIdAsync(dto.UserId);
            var complaintCategory = await _complaintCategoryRepo.GetByIdAsync(dto.CategoryId);
            await _notificationService.CreateNotification(
                                       hostelId: dto.HostelId,
                                       userId: dto.UserId,
                                       title: "New Complaint Raised",
                                       message: $"{user?.Name ?? string.Empty} raised a complaint: {complaintCategory?.CategoryName ?? string.Empty}",
                                       type: "Complaint");
            return Map(complaint);
        }

        public async Task<List<ComplaintResponseDto>> GetAsync(ComplaintFilterDto filter)
        {
            var complaints = await _complaintRepo.GetByFilterAsync(filter);
            return complaints.Select(Map).ToList();
        }

        public async Task<bool> UpdateStatusAsync(Guid complaintId, UpdateComplaintStatusDto dto)
        {
            var complaint = await _complaintRepo.GetByIdAsync(complaintId);
            if (complaint == null) return false;

            complaint.Status = dto.Status;
            await _complaintRepo.UpdateAsync(complaint);
            return true;
        }

        private static ComplaintResponseDto Map(Complaint complaint)
        {
            return new ComplaintResponseDto
            {
                Id = complaint.Id,
                HostelId = complaint.HostelId,
                UserId = complaint.UserId,
                RoomId = complaint.RoomId,
                CategoryId = complaint.CategoryId,
                Status = complaint.Status,
                Content = complaint.Content,
                CreatedAt = complaint.CreatedAt
            };
        }
    }
}
