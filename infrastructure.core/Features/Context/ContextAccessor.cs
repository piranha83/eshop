using System.Security.Claims;
using Infrastructure.Core.Features.Entity;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Core.Features.Context;

///<inheritdoc/>
public class ContextAccessor(IHttpContextAccessor? httpContextAccessor) : IContextAccessor
{
    ///<inheritdoc/>    
    public User? GetUser()
    {
        if (Guid.TryParse(httpContextAccessor?.HttpContext?.User.FindFirstValue("sub"), out Guid externalId) && externalId != Guid.Empty)
        {
            return new User { UserId = externalId };
        }

        return null;
    }

    ///<inheritdoc/>  
    public void SetTerm(SearchCriteria searchCriteria)
    {
        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            searchCriteria.SetTerm(httpContext.Request.Query);
        }
    }

    ///<inheritdoc/> 
    public void SetTotal(long total)
    {
        var httpContext = httpContextAccessor?.HttpContext;
        if (httpContext != null)
        {
            httpContext.Response.Headers.Append("x-total-count", total.ToString());
        }
    }
}