using FluentValidation;
using Infrastructure.Core.Features.Entity;

namespace Catalog.Api.Features;

///<inheritdoc/>
public class SearchCriteriaValidator : AbstractValidator<SearchCriteria>
{
    public SearchCriteriaValidator()
    {
        RuleFor(x => x.Skip).GreaterThanOrEqualTo(1).When(x => x.Skip != null);
        RuleFor(x => x.Take).GreaterThanOrEqualTo(1).When(x => x.Take != null);
        RuleFor(x => x.SortField).NotEmpty().When(x => x.SortField != null);
        RuleFor(x => x.Include).NotEmpty().When(x => x.Include != null);
    }
}
