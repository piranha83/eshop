namespace Identity.Api.Featres.Flow;

/// <summary>
/// Сервис авторизации.
/// </summary>
public interface IFlowService
{
    /// <summary>
    /// Выдать токен.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача.</returns>
    Task Token(CancellationToken cancellationToken = default);

    /// <summary>
    /// Удалить старые токены.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Сколько было удалено.</returns>
    ValueTask<long> End(CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавить приложение для авторизации.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача.</returns>
    Task AddApplication(CancellationToken cancellationToken = default);
}