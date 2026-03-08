using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Core.Features.Security;

///<inheritdoc/>
internal sealed class SecurityContext : ISecurityContext, IDisposable
{
    private ECDsa? signingAlgorithm;
    private bool disposed;
    private readonly IConfiguration configuration;

    public SecurityContext(IConfiguration configuration)
    {
        signingAlgorithm = ECDsa.Create();
        if (!string.IsNullOrWhiteSpace(configuration["signing"]))/*client validation*/
        {
            signingAlgorithm.ImportFromPem(configuration["signing"]);
        }
        this.configuration = configuration;
    }

    ///<inheritdoc/>
    public AsymmetricSecurityKey[] CreateSigningKeys() =>
    [
        new ECDsaSecurityKey(signingAlgorithm)
        {
            KeyId = "Identity.Api.Sign.v2",
        },
    ];

    ///<inheritdoc/>
    public SymmetricSecurityKey[] CreateEncryptionKeys() =>
    [
        new SymmetricSecurityKey(Convert.FromBase64String(configuration["encryption"]!))
        {
            KeyId = "Identity.Api.Enc.v2",
        },
    ];

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        signingAlgorithm?.Dispose();
        signingAlgorithm = null;

        GC.SuppressFinalize(this);
    }
}