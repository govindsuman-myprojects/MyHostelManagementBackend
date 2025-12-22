using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class BedService : IBedService
    {
        private readonly IBedRepository _bedRepo;

        public BedService(IBedRepository bedRepo)
        {
            _bedRepo = bedRepo;
        }

        public async Task<Bed> AddBedAsync(BedDto dto)
        {
            return await _bedRepo.AddBedAsync(dto);
        }

        public async Task<IEnumerable<Bed>> GetAllBedsByRoomIdAsync(Guid roomId)
        {
            return await _bedRepo.GetAllBedsByRoomIdAsync(roomId);
        }

        public async Task<IEnumerable<Bed?>> GetAllBedsForHostelAsync(bool? occupied, Guid hostelId)
        {
            return await _bedRepo.GetAllBedsForHostelAsync(occupied, hostelId);
        }

        public async Task<bool> UpdateBedAsync(Guid bedId, BedDto dto)
        {
            return await _bedRepo.UpdateBedAsync(bedId, dto);
        }

        public async Task<bool> DeleteBedAsync(Guid bedId)
        {
            return await _bedRepo.DeleteBedAsync(bedId);
        }
    }
}
