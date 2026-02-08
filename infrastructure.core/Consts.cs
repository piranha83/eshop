namespace Infrastructure.Core;

public static class Consts
{
    public static readonly string AuthenticateUrl = "oauth/code";
    public static readonly string TokenEndpointUrl = "oauth/token";
    public static readonly string UnauthorizeUrl = "oauth/end";
    public static readonly string AuthorizationFlowError = "Identity flow error";
    public static readonly string ApplicationOrigin = "Application:Origin";
    public static readonly int UsersUnblockTimeMinutes = 15;

    public static class ClaimsRole
    {
        public const string CrudRoleName = "CRUD";
    }
}