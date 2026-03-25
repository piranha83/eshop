using Infrastructure.Core.Extensions;
using Order.Api.DatabaseContext;

namespace Order.Api.Featres.Job;

///<inheritdoc/>
internal partial class InitJob(
    IServiceProvider serviceProvider,
    ILogger<InitJob> logger)
    : BackgroundService
{
    ///<inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Начало задачи...");

            using var scope = serviceProvider.CreateScope();
            await scope.EnsureCreated<ApplicationDbContext>();

            logger.LogInformation($"Задача завершена.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка задачи.");
        }
    }
}