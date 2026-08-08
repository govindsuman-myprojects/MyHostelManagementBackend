using Microsoft.AspNetCore.Mvc;
using MyHostelManagement.DTOs;
using MyHostelManagement.Services.Interfaces;

namespace MyHostelManagement.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // CREATE EXPENSE
        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseDto dto)
        {
            var result = await _expenseService.CreateAsync(dto);
            return Ok(result);
        }

        // GET EXPENSES (FILTER)
        [HttpPost("search")]
        public async Task<IActionResult> Get(ExpenseFilterDto filter)
        {
            return Ok(await _expenseService.GetAsync(filter));
        }

        // UPDATE EXPENSE
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateExpenseDto dto)
        {
            var hostelId = Guid.Parse(User.FindFirst("hostelId")!.Value);
            var result = await _expenseService.UpdateAsync(id, hostelId, dto);
            if (result == null)
                return NotFound("Expense not found");

            return Ok(result);
        }

        // DELETE EXPENSE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var hostelId = Guid.Parse(User.FindFirst("hostelId")!.Value);
            var deleted = await _expenseService.DeleteAsync(id, hostelId);
            if (!deleted)
                return NotFound("Expense not found");

            return Ok("Expense deleted successfully");
        }
    }
}
