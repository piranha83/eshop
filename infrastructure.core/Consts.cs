namespace Infrastructure.Core;

public static class Consts
{
    public static readonly string AuthenticateUrl = "oauth/code";
    public static readonly string TokenEndpointUrl = "oauth/token";
    public static readonly string UnauthorizeUrl = "oauth/end";
    public static readonly string AuthorizationFlowError = "Identity flow error";
    public static readonly string ApplicationOrigin = "Application:Origin";
    public static readonly int UsersUnblockTimeMinutes = 15;

    [Flags]
    public enum ClaimsRoles
    {
        None = 0,
        Viewer = 1 << 0,//1
        Editor = 1 << 1,//2
        Admin = 1 << 2,//4
    }
}