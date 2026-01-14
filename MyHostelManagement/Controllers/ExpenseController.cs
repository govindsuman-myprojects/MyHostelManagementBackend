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
    }
}
