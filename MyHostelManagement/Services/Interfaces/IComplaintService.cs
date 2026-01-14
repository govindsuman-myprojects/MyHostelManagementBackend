using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IComplaintService
    {
        Task<ComplaintResponseDto> CreateAsync(CreateComplaintDto dto);
        Task<List<ComplaintResponseDto>> GetAsync(ComplaintFilterDto filter);
        Task<bool> UpdateStatusAsync(Guid complaintId, UpdateComplaintStatusDto dto);
    }

}
