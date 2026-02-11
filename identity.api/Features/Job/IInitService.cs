namespace Identity.Api.Featres.Job;

/// <summary>
/// Приложение сервиса авторизации.
/// </summary>
public interface IInitService
{
    /// <summary>
    /// Добавить для авторизации.
    /// </summary>
    /// <param name="cancellationToken">Маркер отмены.</param>
    /// <returns>Задача.</returns>
    Task Init(CancellationToken cancellationToken = default);
}