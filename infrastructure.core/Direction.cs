using System.ComponentModel;

namespace Infrastructure.Core;

/// <summary>
/// Фильтры.
/// </summary>
public enum Direction
{
    [Description("По возрастанию")]
    Asc,

    [Description("По убыванию")]
    Desc
}