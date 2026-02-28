using MyHostelManagement.Api.DTOs;
using MyHostelManagement.Api.Models;
using MyHostelManagement.Api.Services.Implementations;
using MyHostelManagement.DTOs;
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


        public PaymentService(IPaymentRepository paymentRepo, IUserService userService, 
            IRoomService roomService)
        {
            _paymentRepo = paymentRepo;
            _userService = userService;
            _roomService = roomService;
        }

        public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto)
        {
            var exists = await _paymentRepo.ExistsAsync(
                dto.UserId, dto.PaymentMonth, dto.PaymentYear);

            if (exists)
                throw new Exception("Payment already exists for this month");

            var payment = new Payment
            {
                HostelId = dto.HostelId,
                UserId = dto.UserId,
                Amount = dto.Amount,
                PaymentMonth = dto.PaymentMonth,
                PaymentYear = dto.PaymentYear
            };

            await _paymentRepo.CreateAsync(payment);
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
                // 🔹 Calculate Due Date for current month
                DateTime dueDate;
                if (user.JoiningDate != null && user.JoiningDate.Value.Day == 1)
                {
                    // If joining date is 1 → due date is last day of previous month
                    dueDate = new DateTime(currentYear, currentMonth, 1).AddDays(-1);
                }
                else
                {
                    int dueDay = user.JoiningDate.Value.Day - 1;

                    // Handle month shorter than due day (Feb case)
                    int daysInMonth = DateTime.DaysInMonth(currentYear, currentMonth);
                    if (dueDay > daysInMonth)
                        dueDay = daysInMonth;

                    dueDate = new DateTime(currentYear, currentMonth, dueDay);
                }

                //// 🔹 Skip if rent not yet due
                //if (today < dueDate)
                //    continue;
                // 🔹 Get total paid for this tenant this month
                var totalPaid = payments.Where(p => p.UserId == user.Id &&
                                p.PaymentMonth == currentMonth &&
                                p.PaymentYear == currentYear)
                                .Sum(p => (decimal?)p.Amount) ?? 0;

                // 🔹 Check if pending
                if (totalPaid < user.RentAmount)
                {
                    var pendingPayment = new PendingPaymentsDto
                    {
                        UserId = user.Id,
                        TenantName = user.Name ?? string.Empty,
                        RoomNumber = rooms.FirstOrDefault(r => r.Id == user.RoomId)?.RoomNumber ?? string.Empty,
                        RentDueDate = dueDate,
                        RentDueAmount = (user.RentAmount ?? 0) - totalPaid
                    };
                    pendingPayments.Add(pendingPayment);
                }
            }
            return pendingPayments.OrderBy(p => p.RentDueDate).ToList();
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
