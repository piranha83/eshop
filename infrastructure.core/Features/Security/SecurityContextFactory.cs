using Microsoft.Extensions.Configuration;

namespace Infrastructure.Core.Features.Security;

public static class SecurityContextFactory
{
    public static ISecurityContext Create(IConfiguration configuration) => new SecurityContext(configuration);
}