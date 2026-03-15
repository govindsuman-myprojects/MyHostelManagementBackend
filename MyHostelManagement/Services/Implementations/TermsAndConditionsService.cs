using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class TermsAndConditionsService : ITermsAndConditionsService
    {
        private readonly ITermsAndConditionsRepository _termsRepository;

        public TermsAndConditionsService(ITermsAndConditionsRepository termsRepository)
        {
            _termsRepository = termsRepository;
        }

        public async Task<TermsResponseDto> CreateOrUpdateAsync(CreateTermsDto dto)
        {
            //var existing = await _termsRepository.GetAsync(dto.HostelId, dto.RoleId);

            //if (existing == null)
            //{
                var terms = new TermsAndConditions
                {
                    HostelId = dto.HostelId,
                    RoleId = dto.RoleId,
                    Content = dto.Content
                };

                await _termsRepository.CreateAsync(terms);
                return Map(terms);
            //}

            //existing.Content = dto.Content;
            //await _termsRepository.UpdateAsync(existing);
            //return Map(existing);
        }

        public async Task<List<TermsResponseDto>> GetAsync(TermsFilterDto filter)
        {
            var terms = await _termsRepository.GetAsync(filter.HostelId, filter.RoleId);
            var response = new List<TermsResponseDto>();
            foreach (var item in terms)
            {
                item.Content = $"● {item.Content}";
                response.Add(Map(item));
            }
            return response;
        }

        private static TermsResponseDto Map(TermsAndConditions terms)
        {
            return new TermsResponseDto
            {
                Id = terms.Id,
                HostelId = terms.HostelId,
                RoleId = terms.RoleId,
                Content = terms.Content,
                CreatedAt = terms.CreatedAt
            };
        }
    }
}
