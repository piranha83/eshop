using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Core.Features.Security;

/// <summary>
/// Контекс.
/// </summary>
public interface ISecurityContext : IDisposable
{
    /// <summary>
    /// Ключ.
    /// </summary>
    SymmetricSecurityKey[] CreateEncryptionKeys();

    /// <summary>
    /// Ключ.
    /// </summary>
    AsymmetricSecurityKey[] CreateSigningKeys();
}