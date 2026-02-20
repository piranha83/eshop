using System.ComponentModel;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Core.Features.Entity;

/// <summary>
/// Фильтры.
/// </summary>
public sealed class SearchCriteria
{
    [FromQuery(Name = "_page")]
    [Description("Страница")]
    public int? Skip { get; set; }

    [FromQuery(Name = "_limit")]
    [Description("Размер")]
    public int? Take { get; set; }

    [FromQuery(Name = "_sort")]
    [Description("Сортировка")]
    public string? SortField { get; set; }

    [FromQuery(Name = "_order")]
    [Description("Порядок")]
    public Direction? SortDirection { get; set; }

    [FromQuery(Name = "_include")]
    [Description("Включить")]
    public string? Include { get; set; }

    [XmlIgnore]
    [Description("Фильтрация")]
    public Dictionary<string, StringValues>? Term { get; private set; }

    public SearchCriteria() { }

    public SearchCriteria(Dictionary<string, StringValues> term) => Term = term;

    public void SetTerm(IQueryCollection? terms)
    {
        Term = terms?
            .Where(x => x.Key != "_page" && x.Key != "_limit" && x.Key != "_sort" && x.Key != "_order" && x.Key != "_include")
            .ToDictionary(x => x.Key, x => x.Value);
    }
}