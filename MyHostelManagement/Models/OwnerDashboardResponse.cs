using Microsoft.AspNetCore.Identity;
using MyHostelManagement.Api.DTOs;

namespace MyHostelManagement.Api.Models;

public class OwnerDashboardResponse
{
    public UserDto? User { get; set; }
    public HostelDto? Hostel { get; set; }
    public StatsDto? Stats { get; set; }
}

//public class UserDto
//{
//    public string? Name { get; set; }
//}

public class StatsDto
{
    public int TotalBeds { get; set; }
    public int VacantBeds { get; set; }
    public int OccupiedBeds { get; set; }
    public decimal PaymentsToday { get; set; }
    public decimal PaymentsRecievedThisMonth { get; set; }
    public decimal PaymentsPendingThisMonth { get; set; }
    public int TotalRooms { get; set; }
    public int VacantRooms { get; set; }
}
