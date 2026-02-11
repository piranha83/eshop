using Identity.Api.DatabaseContext;
using Identity.Api.Featres.Flow;
using Identity.Api.Featres.User;
using Infrastructure.Core;
using OpenIddict.Abstractions;
using static Infrastructure.Core.Consts;

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
        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreatedAsync(cancellationToken);
        
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var flowService = scope.ServiceProvider.GetRequiredService<IInitService>();

        await userService.Add("Admin", "vs6_@>7Xt_Jkkg6r".Hash512(), ClaimsRoles.Admin, cancellationToken);
        await userService.Add("Client", "7k=9r8F".Hash512(), ClaimsRoles.Viewer, cancellationToken);
        await flowService.Init(cancellationToken);
    }

    private async Task Cleanup(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var tokenService = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
        logger.LogInformation($"Отозвано {await tokenService.PruneAsync(DateTimeOffset.UtcNow.AddMinutes(-Consts.UsersUnblockTimeMinutes), cancellationToken)} токенов.");
        logger.LogInformation($"Разблокировано {await userService.Unblock(cancellationToken)} пользователей.");
    }
}