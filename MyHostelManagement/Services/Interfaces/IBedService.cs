using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;

namespace MyHostelManagement.Services.Interfaces
{
    public interface IBedService
    {
        Task<Bed> AddBedAsync(BedDto dto);
        Task<IEnumerable<Bed>> GetAllBedsByRoomIdAsync(Guid roomId);
        Task<IEnumerable<Bed?>> GetAllBedsForHostelAsync(bool? occupied,Guid hostelId);
        Task<bool> UpdateBedAsync(Guid bedId, BedDto dto);
        Task<bool> DeleteBedAsync(Guid bedId);
    }
}
