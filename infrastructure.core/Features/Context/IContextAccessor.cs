using Infrastructure.Core.Features.Entity;

namespace Infrastructure.Core.Features.Context;

/// <summary>
/// Представляет контекст в системе.
/// </summary>
public interface IContextAccessor
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    User? GetUser();

    /// <summary>
    /// Запросы фильтра.
    /// </summary>
    /// <param name="searchCriteria">Фильтры.</param>
    /// <summary>
    void SetTerm(SearchCriteria searchCriteria);

    /// <summary>
    /// Задать количество записей, для moq апи.
    /// </summary>
    /// <param name="total">Количество записей.</param>
    void SetTotal(long total);
}