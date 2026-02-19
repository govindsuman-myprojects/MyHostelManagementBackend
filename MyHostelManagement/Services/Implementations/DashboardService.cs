using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
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
        private readonly IAnnouncementService _announcementService;
        private readonly IAnnouncementTypeService _announcementTypeService;
        private readonly IComplaintCategoryService _complaintCategoryService;

        public DashboardService(IRoomService roomService, IPaymentService paymentService, IUserService userService, 
            IComplaintService complaintService, IExpenseService expenseService, IHostelService hostelService, ITermsAndConditionsService trmsAndConditionsService, 
            IAnnouncementService announcementService, IAnnouncementTypeService announcementTypeService, IComplaintCategoryService complaintCategoryService)
        {
            _roomService = roomService;
            _paymentService = paymentService;
            _userService = userService;
            _complaintService = complaintService;
            _expenseService = expenseService;
            _hostelService = hostelService;
            _trmsAndConditionsService = trmsAndConditionsService;
            _announcementService = announcementService;
            _announcementTypeService = announcementTypeService;
            _complaintCategoryService = complaintCategoryService;
        }

        // OWNER DASHBOARD
        public async Task<OwnerDashboardDto> GetOwnerDashboardAsync(Guid hostelId)
        {
            var today = DateTime.UtcNow.Date;
            var now = DateTime.UtcNow;
            var rooms = await _roomService.GetByHostelAsync(hostelId, "All");

            var hostel = await _hostelService.GetByIdAsync(hostelId);
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

            var paidUserIds = payments
                    .Select(p => p.UserId)
                    .ToHashSet();

            var usersWithoutPayments = users
                .Where(u => !paidUserIds.Contains(u.Id))
                .ToList();
            var pendingPayments = new List<PendingPaymentsDto>();
            foreach (var item in usersWithoutPayments)
            {
                var roomNumber = rooms.FirstOrDefault(r => r.Id == item.RoomId);
                var pendingPayemnt = new PendingPaymentsDto
                {
                    UserId = item.Id,
                    TenantName = item.Name,
                    RoomNumber = roomNumber?.RoomNumber ?? string.Empty,
                    RentDueDate = item.JoiningDate?.AddDays(-1) ?? DateTime.Today,
                    RentDueAmount = (decimal)item.RentAmount
                };
                pendingPayments.Add(pendingPayemnt);
            }
            var pendingComplaintsList = new List<PendingComplaintsDto>();
            var complaintTypes = await _complaintCategoryService.GetAllAsync();
            foreach (var item in pendingComplaints)
            {
                var tenantName = users.FirstOrDefault(x => x.Id == item.UserId);
                var roomNumber = rooms.FirstOrDefault(r => r.Id == item.RoomId);
                var complaintCategory = complaintTypes.FirstOrDefault(r => r.Id == item.CategoryId);
                var pendingComplaint = new PendingComplaintsDto
                {
                    id = item.Id,
                    TenantName = tenantName?.Name,
                    RoomNumber = roomNumber?.RoomNumber,
                    ComplaintCategory = complaintCategory?.CategoryName,
                    Content = item.Content,
                    CreatedDate = item.CreatedAt

                };
                pendingComplaintsList.Add(pendingComplaint);
            }

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
                PendingComplaintCount = pendingComplaints.Count(),
                MonthExpenses = expenses.Sum(x => x.Amount),
                PendingPayments = pendingPayments,
                PendingComplaints = pendingComplaintsList,
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

            var annoucments = await _announcementService.GetAsync(new AnnouncementFilterDto
            {
                HostelId = user.HostelId,
                OnlyActive = true
            });
            var annoucmentTypes = await _announcementTypeService.GetAllAsync();
            var hostelAnnoucments = new List<AnnouncementDto>();
            foreach (var item in annoucments)
            {
                var announcmeneType = annoucmentTypes.FirstOrDefault(r => r.Id == item.TypeId);
                var hostelAnnoucment = new AnnouncementDto
                {
                    Subject = item.Subject,
                    Message = item.Message,
                    StartDate = item.StartDate ?? DateTime.Today,
                    EndDate = item.EndDate ?? DateTime.Today,
                    AnnouncementType = announcmeneType?.TypeName ?? string.Empty
                };
                hostelAnnoucments.Add(hostelAnnoucment);
            }

            var complaintTypes = await _complaintCategoryService.GetAllAsync();
            var PendingComplaints = new List<ComplaintDto>();
            foreach (var item in pendingComplaints)
            {
                var complaintType = complaintTypes.FirstOrDefault(r => r.Id == item.CategoryId);
                var complaintDto = new ComplaintDto
                {
                    Content = item.Content ?? string.Empty,
                    CreatedAt = item.CreatedAt,
                    ComplaintType = complaintType?.CategoryName ?? string.Empty
                };
                PendingComplaints.Add(complaintDto);
            }

            return new TenantDashboardDto
            {
                    TenantName = user?.Name ?? string.Empty,
                    HostelName = hostel?.Name ?? string.Empty,
                    RoomNumber = room?.RoomNumber ?? string.Empty,
                    TermsAndConditions = termsAndConditions?.Content ?? string.Empty,
                    RentDueDate = rentDue == 0 ? user?.JoiningDate?.AddDays(-1).AddMonths(1) : user?.JoiningDate?.AddDays(-1),
                    RentDue = rentDue ?? 0,
                    Announcements = hostelAnnoucments,
                    Complaints = PendingComplaints
            };
        }
    }
}
