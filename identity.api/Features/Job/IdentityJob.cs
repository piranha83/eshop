using Identity.Api.Featres.Flow;
using Identity.Api.Featres.User;
using Infrastructure.Core;
using Infrastructure.Core.Abstractions;
using OpenIddict.Abstractions;

namespace Identity.Api.Featres.Job;

///<inheritdoc/>
internal partial class IdentityJob(
    IServiceProvider serviceProvider,
    ILogger<IdentityJob> logger)
    : BackgroundService
{
    ///<inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Начало задачи...");

            await InitDb(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Cleanup(stoppingToken);
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                logger.LogInformation($"Ожидание...");
            }

            logger.LogInformation($"Задача завершена.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка задачи.");
        }
    }

    private async Task InitDb(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        var flowService = scope.ServiceProvider.GetRequiredService<IFlowService>();

        await userService.Add("Admin", "vs6_@^>7X;~,t$B#_Jkkg6r".Hash512(), cancellationToken);
        await userService.Add("Client", "7k=9r8F".Hash512(), cancellationToken);
        await flowService.AddApplication(cancellationToken);
    }

    private async Task Cleanup(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var userService = scope.ServiceProvider.GetRequiredService<UserService>();
        var tokenService = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
        logger.LogInformation($"Отозвано {await tokenService.PruneAsync(DateTimeOffset.UtcNow.AddMinutes(-Consts.UsersUnblockTimeMinutes), cancellationToken)} токенов.");
        logger.LogInformation($"Разблокировано {await userService.Unblock(cancellationToken)} пользователей.");
    }
}