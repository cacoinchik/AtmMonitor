using AtmMonitor.API.DTOs;
using AtmMonitor.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AtmMonitor.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ATMsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ATMsController(AppDbContext db)
        {
            _db = db;
        }

        //Получить список всех банкоматов
        [HttpGet]
        public async Task<IActionResult> GetATMs(CancellationToken cancellationToken = default)
        {
            try
            {
                var atms = await _db.ATMs
                .AsNoTracking()
                .Select(a => new ATMDto
                {
                    Id = a.Id,
                    Address = a.Address,
                    InstallationDate = a.InstallationDate,
                })
                .ToListAsync(cancellationToken);

                return Ok(atms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении данных: {ex.Message}");
            }
        }

        //Получить банкомат по Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetATMById(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var atm = await _db.ATMs
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new ATMDto
                {
                    Id = a.Id,
                    Address = a.Address,
                    InstallationDate = a.InstallationDate
                })
                .FirstOrDefaultAsync(cancellationToken);

                if (atm is null)
                    return NotFound();

                return Ok(atm);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка получения данных: {ex.Message}");
            }
        }
    }
}
