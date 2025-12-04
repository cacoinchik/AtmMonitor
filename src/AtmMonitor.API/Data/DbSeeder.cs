using AtmMonitor.Core.Entities;
using AtmMonitor.Core.Enums;
using AtmMonitor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AtmMonitor.API.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILogger logger)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                logger.LogInformation("Начало применения миграций...");

                await context.Database.MigrateAsync();

                logger.LogInformation("Миграции применены успешно");

                if (await context.ATMs.AnyAsync())
                {
                    logger.LogInformation("База данных уже содержит данные. Seed пропущен.");
                    return;
                }

                logger.LogInformation("Начало заполнения базы данных тестовыми данными...");

                var atms = new List<ATM>
                {
                    new ATM { Address = "г. Москва, ул. Тверская, д. 10", InstallationDate = new DateTime(2020, 5, 15) },
                    new ATM { Address = "г. Санкт-Петербург, Невский пр., д. 28", InstallationDate = new DateTime(2019, 8, 20) },
                    new ATM { Address = "г. Москва, ул. Арбат, д. 15", InstallationDate = new DateTime(2021, 3, 10) },
                    new ATM { Address = "г. Казань, ул. Баумана, д. 25", InstallationDate = new DateTime(2022, 1, 5) },
                    new ATM { Address = "г. Санкт-Петербург, ул. Рубинштейна, д. 7", InstallationDate = new DateTime(2020, 11, 12) }
                };

                await context.ATMs.AddRangeAsync(atms);
                await context.SaveChangesAsync();

                logger.LogInformation("Добавлено {Count} банкоматов", atms.Count);

                var random = new Random(42);
                var transactions = new List<Transaction>();
                var startDate = new DateTime(2024, 11, 1);
                var endDate = new DateTime(2024, 12, 31);

                foreach (var atm in atms)
                {
                    // 20 транзакций на банкомат
                    for (int i = 0; i < 20; i++)
                    {
                        var daysToAdd = random.Next(0, (endDate - startDate).Days);
                        var hoursToAdd = random.Next(0, 24);
                        var minutesToAdd = random.Next(0, 60);

                        transactions.Add(new Transaction
                        {
                            ATMId = atm.Id,
                            OperationDate = startDate.AddDays(daysToAdd).AddHours(hoursToAdd).AddMinutes(minutesToAdd),
                            Type = random.Next(2) == 0 ? TransactionType.Withdrawal : TransactionType.Deposit,
                            Amount = random.Next(500, 50000)
                        });
                    }
                }

                await context.Transactions.AddRangeAsync(transactions);
                await context.SaveChangesAsync();

                logger.LogInformation("Добавлено {Count} транзакций", transactions.Count);
                logger.LogInformation("База данных успешно заполнена тестовыми данными");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при применении миграций или заполнении базы данных");
                throw;
            }
        }
    }
}