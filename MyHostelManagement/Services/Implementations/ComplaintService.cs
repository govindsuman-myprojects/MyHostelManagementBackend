using AutoMapper;
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

        public ComplaintService(IMapper mapper, IComplaintRepository complaintRepo)
        {
            _mapper = mapper;
            _complaintRepo = complaintRepo;
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
