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
}