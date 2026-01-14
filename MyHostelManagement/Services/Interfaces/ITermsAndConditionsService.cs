using MyHostelManagement.DTOs;

namespace MyHostelManagement.Services.Interfaces
{
    public interface ITermsAndConditionsService
    {
        Task<TermsResponseDto> CreateOrUpdateAsync(CreateTermsDto dto);
        Task<TermsResponseDto?> GetAsync(TermsFilterDto filter);
    }
}
