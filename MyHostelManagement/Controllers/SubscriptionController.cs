using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHostelManagement.Models;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }

        [HttpGet("plans")]
        public async Task<IActionResult> GetPlans()
        {
            var plans = await _subscriptionService.GetPlans();
            return Ok(plans);
        }

        [HttpGet("current/{hostelId}")]
        public async Task<IActionResult> GetCurrentSubscription(Guid hostelId)
        {
            var subscription = await _subscriptionService.GetCurrentSubscription(hostelId);

            if (subscription == null)
                return NotFound("No active subscription");

            var daysRemaining = (subscription.EndDate - DateTime.UtcNow).Days;

            return Ok(new
            {
                subscription.Id,
                subscription.Plan.PlanName,
                subscription.Plan.Price,
                subscription.StartDate,
                subscription.EndDate,
                daysRemaining
            });
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe(Guid hostelId, Guid userId, Guid planId)
        {

            var plans = await _subscriptionService.GetPlans();
            var plan = plans.Where(x => x.Id == planId).FirstOrDefault();
            if (plan == null)
                return BadRequest("Invalid Plan");

            // Expire old subscription if exists
            var oldSubscription = await _subscriptionService.GetCurrentSubscription(hostelId);
            if (oldSubscription != null)
                oldSubscription.Status = "Expired";

            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(plan.DurationInDays);

            var newSubscription = new HostelSubscription
            {
                Id = Guid.NewGuid(),
                HostelId = hostelId,
                UserId = userId,
                PlanId = planId,
                StartDate = startDate,
                EndDate = endDate,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            await _subscriptionService.AddHostelSubscription(newSubscription);

            var payment = new SubscriptionPayment
            {
                Id = Guid.NewGuid(),
                SubscriptionId = newSubscription.Id,
                HostelId = hostelId,
                UserId = userId,
                Amount = plan.Price,
                PaymentMode = "Manual",
                TransactionId = Guid.NewGuid().ToString(),
                PaymentStatus = "Paid",
                PaymentDate = DateTime.UtcNow
            };

            await _subscriptionService.AddSubscriptionPayment(payment);
            return Ok("Subscription Activated Successfully");
        }

        [HttpGet("payments/{hostelId}")]
        public async Task<IActionResult> GetPaymentHistory(Guid hostelId)
        {
            var payments = await _subscriptionService.GetPaymentHistory(hostelId);
            return Ok(payments);
        }

        [HttpPost("check-expiry/{hostelId}")]
        public async Task<IActionResult> CheckExpiry(Guid hostelId)
        {
            var subscription = await _subscriptionService.GetCurrentSubscription(hostelId);
            if (subscription == null)
                return NotFound("No active subscription");

            if (subscription.EndDate < DateTime.UtcNow)
            {
                subscription.Status = "Expired";
                await _subscriptionService.AddHostelSubscription(subscription);
                return Ok("Subscription Expired");
            }

            return Ok("Subscription Active");
        }

        [HttpGet("validate/{hostelId}")]
        public async Task<IActionResult> ValidateSubscription(Guid hostelId)
        {
            var subscription = await _subscriptionService.GetCurrentSubscription(hostelId);

            if (subscription == null || subscription.EndDate < DateTime.UtcNow)
                return Unauthorized("Subscription Expired");

            return Ok("Valid");
        }
    }
}
