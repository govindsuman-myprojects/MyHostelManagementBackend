using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.Data;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Repositories.Interfaces;
using System.Reflection.Metadata;

namespace MyHostelManagement.Repositories.Implementations
{
    public class BedRepository : IBedRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public BedRepository(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public virtual async Task<Bed> AddBedAsync(BedDto dto)
        {
            var bed = _mapper.Map<Bed>(dto);
            await _dbContext.Beds.AddAsync(bed);
            await _dbContext.SaveChangesAsync();
            return bed;
        }

        public virtual async Task<IEnumerable<Bed>> GetAllBedsByRoomIdAsync(Guid roomId)
        {
            var response = await _dbContext.Beds
                                .Where(x => x.RoomId == roomId).ToListAsync();

            return response;
        }

        public virtual async Task<IEnumerable<Bed?>> GetAllBedsForHostelAsync(bool? occupied,Guid hostelId)
        {
            var bedResponse = new List<Bed>();
            if (occupied == null)
            {
                bedResponse = await _dbContext.Beds
                                .Where(x => x.HostelId == hostelId).ToListAsync();
            }
            else if ((bool)occupied)
            {
                bedResponse = await _dbContext.Beds
                                .Where(x => x.HostelId == hostelId && x.Status == "occupied").ToListAsync();
            }
            else
            {
                bedResponse = await _dbContext.Beds
                                .Where(x => x.HostelId == hostelId && x.Status == "avaliable").ToListAsync();
            }
            return bedResponse;
        }

        public virtual async Task<bool> UpdateBedAsync(Guid bedId, BedDto dto)
        {
            var bed = await _dbContext.Beds.Where(x => x.Id == bedId).FirstOrDefaultAsync();
            if (bed == null)
                return false;

            bed.Id = bedId;
            bed.BedNumber = dto.BedNumber;
            bed.Status = dto.Status;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> DeleteBedAsync(Guid bedId)
        {
            var tenant = await _dbContext.Tenants
                                .Where(x => x.BedId == bedId).FirstOrDefaultAsync();

            if (tenant != null)
                return false;

            else if (tenant == null)
            {
                var deleteBed = await _dbContext.Beds.Where(x => x.Id == bedId).FirstOrDefaultAsync();
                
                if (deleteBed == null)
                    return false;

                _dbContext.Beds.Remove(deleteBed);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
