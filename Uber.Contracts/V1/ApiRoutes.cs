namespace Uber.Contract.V1;

public class ApiRoutes
{
    private const string Version = "v1";
    private const string Root = "api";
    public const string Base = $"{Root}/{Version}";
    
    public static class Identity
    {
        public const string Login = Base + "/identity/login";
        public const string Register = Base + "/identity/register";
        public const string Refresh = Base + "/identity/refresh";
    }
    public static class Driver
    {
        public const string Login = Base + "/driver/login";
        public const string Register = Base + "/driver/register";
        public const string RefreshToken = Base + "/driver/RefreshToken";
    }
}