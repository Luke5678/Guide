namespace Guide.Domain.Common;

public static class UserRoles
{
    public const string Administrator = "Admin";
    public const string MainAdministrator = "MainAdmin";

    public static string[] List => [Administrator, MainAdministrator];
}