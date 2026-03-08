
namespace Infrastructure.Core.Features.Entity;

/// <summary>
/// Обработка апи.
/// </summary>
public sealed class ValidationApiException: Exception
{
    public IDictionary<string, string[]> Details { get; private set; }

    public ValidationApiException(IDictionary<string, string[]> details)
    {
        Details = details;
    }
}