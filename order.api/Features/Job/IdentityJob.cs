using Infrastructure.Core.Extensions;
using Order.Api.DatabaseContext;

namespace Order.Api.Featres.Job;

///<inheritdoc/>
internal partial class OrderJob(
    IServiceProvider serviceProvider,
    ILogger<OrderJob> logger)
    : BackgroundService
{
    ///<inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Начало задачи...");

            await InitDb();

            logger.LogInformation($"Задача завершена.");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation($"Задача отменена.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка задачи.");
        }
    }

    private async Task InitDb()
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        await scope.EnsureCreated<ApplicationDbContext>();
    }
}