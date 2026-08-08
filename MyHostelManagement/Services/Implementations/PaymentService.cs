using MyHostelManagement.DTOs;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Implementations;
using MyHostelManagement.Repositories.Implementations;
using MyHostelManagement.Repositories.Interfaces;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly INotificationService _notificationService;

        public PaymentService(IPaymentRepository paymentRepo, IUserService userService,
            IRoomService roomService, INotificationService notificationService)
        {
            _paymentRepo = paymentRepo;
            _userService = userService;
            _roomService = roomService;
            _notificationService = notificationService;
        }

        public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto)
        {
            var exists = await _paymentRepo.ExistsAsync(
                dto.UserId, dto.PaymentMonth, dto.PaymentYear);

            if (exists)
                throw new ApiException("Payment already exists for this month", StatusCodes.Status409Conflict);

            var payment = new Payment
            {
                HostelId = dto.HostelId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                PaymentMonth = dto.PaymentMonth,
                PaymentYear = dto.PaymentYear
            };

            var user = await _userService.GetByIdAsync(dto.UserId);
            await _paymentRepo.CreateAsync(payment);
            await _notificationService.CreateNotification(
                                       hostelId: dto.HostelId,
                                       userId: dto.UserId,
                                       title: "New Payment Received",
                                       message: $"₹{payment.Amount} received from {user?.Name ?? string.Empty}",
                                       type: "Payment");
            return Map(payment);
        }

        public async Task<List<PaymentResponseDto>> GetAsync(PaymentFilterDto filter)
        {
            var payments = await _paymentRepo.GetByFilterAsync(filter);
            return payments.Select(Map).ToList();
        }

        private static PaymentResponseDto Map(Payment payment)
        {
            return new PaymentResponseDto
            {
                Id = payment.Id,
                UserId = payment.UserId,
                Amount = payment.Amount,
                PaymentMonth = payment.PaymentMonth,
                PaymentYear = payment.PaymentYear,
                CreatedAt = payment.CreatedAt,
            };
        }

        public async Task<List<PaymentResponseDto>> GetByHostelId(Guid hostelId)
        {
            var payments = await _paymentRepo.GetByHostelId(hostelId);
            return payments.Select(Map).ToList();
        }

        public async Task<List<PendingPaymentsDto>> GetPendingPayments(Guid hostelId)
        {
            var users = await _userService.GetTenantsAsync(hostelId);
            var filterPaymentDto = new PaymentFilterDto
            {
                HostelId = hostelId,
                Month = DateTime.UtcNow.Month,
                Year = DateTime.UtcNow.Year
            };
            var payments = await GetAsync(filterPaymentDto);
            var rooms = await _roomService.GetByHostelAsync(hostelId, "All");

            return await GetPendingPaymentsAsync(users, payments, rooms);
        }

        public async Task<List<PendingPaymentsDto>> GetPendingPaymentsAsync(List<UserResponseDto> users, List<PaymentResponseDto> payments, List<RoomResponseDto> rooms)
        {
            var today = DateTime.UtcNow.Date;
            int currentMonth = today.Month;
            int currentYear = today.Year;
            var pendingPayments = new List<PendingPaymentsDto>();

            foreach (var user in users)
            {
                if (!user.JoiningDate.HasValue)
                    continue;

                var dueDate = ComputeDueDate(user.JoiningDate.Value, currentYear, currentMonth);

                // Tenant joined after this period's due date — first month not yet owed
                if (user.JoiningDate.Value.Date > dueDate.Date)
                    continue;

                // Due date hasn't arrived yet this month
                if (today < dueDate)
                    continue;

                var totalPaid = payments
                    .Where(p => p.UserId == user.Id &&
                                p.PaymentMonth == currentMonth &&
                                p.PaymentYear == currentYear)
                    .Sum(p => (decimal?)p.Amount) ?? 0;

                if (totalPaid < user.RentAmount)
                {
                    pendingPayments.Add(new PendingPaymentsDto
                    {
                        UserId = user.Id,
                        TenantName = user.Name ?? string.Empty,
                        RoomNumber = rooms.FirstOrDefault(r => r.Id == user.RoomId)?.RoomNumber ?? string.Empty,
                        RentDueDate = dueDate,
                        RentDueAmount = (user.RentAmount ?? 0) - totalPaid
                    });
                }
            }

            return pendingPayments.OrderBy(p => p.RentDueDate).ToList();
        }

        public async Task<List<TenantPaymentStatusDto>> GetTenantPaymentStatusAsync(Guid hostelId)
        {
            var today = DateTime.UtcNow.Date;
            int currentMonth = today.Month;
            int currentYear = today.Year;

            var users = await _userService.GetTenantsAsync(hostelId);
            var payments = await GetAsync(new PaymentFilterDto
            {
                HostelId = hostelId,
                Month = currentMonth,
                Year = currentYear
            });
            var rooms = await _roomService.GetByHostelAsync(hostelId, "All");

            var result = new List<TenantPaymentStatusDto>();

            foreach (var user in users)
            {
                if (!user.JoiningDate.HasValue)
                    continue;

                var dueDate = ComputeDueDate(user.JoiningDate.Value, currentYear, currentMonth);

                // Tenant joined after this period's due date — not liable yet for this month
                if (user.JoiningDate.Value.Date > dueDate.Date)
                    continue;

                var amountPaid = payments
                    .Where(p => p.UserId == user.Id)
                    .Sum(p => p.Amount);

                result.Add(new TenantPaymentStatusDto
                {
                    UserId = user.Id,
                    TenantName = user.Name ?? string.Empty,
                    RoomNumber = rooms.FirstOrDefault(r => r.Id == user.RoomId)?.RoomNumber ?? string.Empty,
                    RentAmount = user.RentAmount ?? 0,
                    RentDueDate = dueDate,
                    IsPaid = amountPaid >= (user.RentAmount ?? 0),
                    AmountPaid = amountPaid
                });
            }

            // Unpaid first, then paid; within each group ordered by due date
            return result
                .OrderBy(r => r.IsPaid)
                .ThenBy(r => r.RentDueDate)
                .ToList();
        }

        private static DateTime ComputeDueDate(DateTime joinDate, int year, int month)
        {
            int joinDay = joinDate.Day;
            if (joinDay == 1)
                return new DateTime(year, month, 1).AddDays(-1);
            int dueDay = Math.Min(joinDay - 1, DateTime.DaysInMonth(year, month));
            return new DateTime(year, month, dueDay);
        }

        public async Task<List<PendingPaymentsDto>> GetRecievedPayments(Guid hostelId)
        {
            var users = await _userService.GetTenantsAsync(hostelId);
            var filterPaymentDto = new PaymentFilterDto
            {
                HostelId = hostelId,
                Month = DateTime.UtcNow.Month,
                Year = DateTime.UtcNow.Year
            };
            var payments = await GetAsync(filterPaymentDto);
            var rooms = await _roomService.GetByHostelAsync(hostelId, "All");
            var responeList = new List<PendingPaymentsDto>();
            if (payments.Any())
            {
                foreach (var payment in payments)
                {
                    var user = users.Where(x => x.Id == payment.UserId).FirstOrDefault();
                    var room = rooms.Where(x => x.Id == user?.RoomId).FirstOrDefault();
                    var pendingPayment = new PendingPaymentsDto
                    {
                        RentDueAmount = payment.Amount,
                        RentDueDate = payment.CreatedAt,
                        RoomNumber = room?.RoomNumber ?? string.Empty,
                        TenantName = user?.Name ?? string.Empty,
                        UserId = payment.UserId,
                    };
                    responeList.Add(pendingPayment);
                }
            }
            return responeList;
        }
    }
}
