using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Services.Interfaces;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IRoomService _roomService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IComplaintService _complaintService;
        private readonly IExpenseService _expenseService;
        private readonly IHostelService _hostelService;
        private readonly ITermsAndConditionsService _trmsAndConditionsService;

        public DashboardService(IRoomService roomService, IPaymentService paymentService, IUserService userService, 
            IComplaintService complaintService, IExpenseService expenseService, IHostelService hostelService, ITermsAndConditionsService trmsAndConditionsService)
        {
            _roomService = roomService;
            _paymentService = paymentService;
            _userService = userService;
            _complaintService = complaintService;
            _expenseService = expenseService;
            _hostelService = hostelService;
            _trmsAndConditionsService = trmsAndConditionsService;
        }

        // OWNER DASHBOARD
        public async Task<OwnerDashboardDto> GetOwnerDashboardAsync(Guid hostelId)
        {
            var today = DateTime.UtcNow.Date;
            var now = DateTime.UtcNow;

            var hostel = await _hostelService.GetByIdAsync(hostelId);
            var rooms = await _roomService.GetByHostelAsync(hostelId);
            var totalBeds = rooms.Sum(r => r.TotalBeds);
            var occupiedBeds = rooms.Sum(x => x.OccupiedBeds);

            var filterPaymentDto = new PaymentFilterDto
            {
                HostelId = hostelId,
            };
            var payments = await _paymentService.GetAsync(filterPaymentDto);
            var todayReceivedPayments = payments.Where(p => p.CreatedAt.Date == DateTime.Today).Sum(p => p.Amount);
            var monthReceivedPayments = payments.Where(p => p.PaymentMonth == now.Month && p.PaymentYear == now.Year).Sum(p => p.Amount);
            var users = await _userService.GetTenantsAsync(hostelId);
            var monthTotalPayments = users.Sum(x => x.RentAmount);

            var filterComplaintDto = new ComplaintFilterDto
            {
                HostelId = hostelId,
                Status = 1
            };
            var pendingComplaints = await _complaintService.GetAsync(filterComplaintDto);

            var filterExpenseDto = new ExpenseFilterDto
            {
                HostelId = hostelId,
                Month = now.Month,
            };
            var expenses = await _expenseService.GetAsync(filterExpenseDto);

            return new OwnerDashboardDto
            {
                OwnerName = hostel?.OwnerName ?? string.Empty,
                HostelName = hostel?.Name ?? string.Empty,
                TotalBeds = totalBeds,
                OccupiedBeds = occupiedBeds,
                VacantBeds = totalBeds - occupiedBeds,
                TodayReceivedPayments = todayReceivedPayments,
                MonthReceivedPayments = monthReceivedPayments,
                MonthPendingPayments = (decimal)(monthTotalPayments - monthReceivedPayments),
                PendingComplaints = pendingComplaints.Count(),
                MonthExpenses = expenses.Sum(x => x.Amount)
            };
        }

        // TENANT DASHBOARD
        public async Task<TenantDashboardDto> GetTenantDashboardAsync(Guid userId)
        {
            var now = DateTime.UtcNow;
            var user = await _userService.GetByIdAsync(userId);
            var hostel = await _hostelService.GetByIdAsync(user.HostelId);
            var room = await _roomService.GetByIdAsync(user.RoomId ?? new Guid());

            var filterComplaintDto = new ComplaintFilterDto
            {
                HostelId = hostel.Id,
                Status = 1,
                UserId = userId,
            };
            var pendingComplaints = await _complaintService.GetAsync(filterComplaintDto);

            var filterTermsAndConsitions = new TermsFilterDto
            {
                HostelId = user.HostelId,
            };
            var termsAndConditions = await _trmsAndConditionsService.GetAsync(filterTermsAndConsitions);

            var filterPayments = new PaymentFilterDto
            {
                HostelId = user.HostelId,
                Month = now.Month,
                UserId = userId,
                Year = now.Year,
            };

            var payment = await _paymentService.GetAsync(filterPayments);
            decimal? rentDue = 0;

            var paidAmount = payment?.Sum(x => x.Amount) ?? 0;

            if (paidAmount <= 0)
            {
                rentDue = user.RentAmount;
            }
            else if (paidAmount >= user.RentAmount)
            {
                rentDue = 0;
            }
            else
            {
                rentDue = user.RentAmount - paidAmount;
            }

            return new TenantDashboardDto
                {
                    TenantName = user?.Name ?? string.Empty,
                    HostelName = hostel?.Name ?? string.Empty,
                    RoomNumber = room?.RoomNumber ?? string.Empty,
                    PendingComplaints = pendingComplaints.Count(),
                    TermsAndConditions = termsAndConditions?.Content ?? string.Empty,
                    RentDueDate = rentDue == 0 ? user?.JoiningDate?.AddDays(-1).AddMonths(1) : user?.JoiningDate?.AddDays(-1),
                    RentDue = rentDue ?? 0
                };
        }
    }
}
