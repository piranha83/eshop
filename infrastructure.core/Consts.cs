namespace Infrastructure.Core;

public static class Consts
{
    public static readonly string AuthorizationUrl = "oauth/authorize";
    public static readonly string TokenEndpointUrl = "oauth/token";
    public static readonly string UnauthorizeUrl = "oauth/unauthorize";
    public static readonly string CallbackUrl = "oauth/callback";
    public static readonly string ApplicationOrigin = "Application:Origin";

    public static class ClaimsRole
    {
        public const string CrudRoleName = "CRUD";
    }
}