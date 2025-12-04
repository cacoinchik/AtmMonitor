using AtmMonitor.API.DTOs;
using AtmMonitor.Core.Enums;
using AtmMonitor.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtmMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TransactionsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Получение транзакций по банкоматам
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] TransactionFilterRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                if (request is null)
                    return BadRequest("Запрос пуст");

                if (request.DateFrom.HasValue && request.DateTo.HasValue && request.DateFrom > request.DateTo)
                    return BadRequest("Дата От не может быть бльше даты До");

                var transactions = _db.Transactions.AsNoTracking().Include(t => t.ATM).AsQueryable();

                if (request.ATMIds != null && request.ATMIds.Any())
                {
                    var existingATMIds = await _db.ATMs
                        .Where(a => request.ATMIds.Contains(a.Id))
                        .Select(a => a.Id)
                        .ToListAsync(cancellationToken);
                    var nonExistenIds = request.ATMIds.Except(existingATMIds).ToList();
                    if (nonExistenIds.Any())
                    {
                        return BadRequest(new
                        {
                            ErrorMessage = "Некоторые банкоматы не найдены",
                            NonExistenIds = nonExistenIds
                        });
                    }

                    transactions = transactions.Where(t => request.ATMIds.Contains(t.ATMId));
                }

                if (request.DateFrom.HasValue)
                {
                    transactions = transactions.Where(t => t.OperationDate >= request.DateFrom.Value);
                }

                if (request.DateTo.HasValue)
                {
                    transactions = transactions.Where(t => t.OperationDate <= request.DateTo.Value);
                }

                transactions = transactions.OrderByDescending(t => t.OperationDate);

                var result = await transactions
                    .Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        OperationDate = t.OperationDate,
                        ATMId = t.ATMId,
                        ATMAddress = t.ATM.Address,
                        Type = t.Type == TransactionType.Withdrawal ? "Снятие наличных" : "Внесение наличных",
                        Amount = t.Amount
                    })
                    .ToListAsync(cancellationToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении транзакций: {ex.Message}");
            }
        }
    }
}
