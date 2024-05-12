namespace Guide.Shared.Common.Static;

public static class LanguageCodes
{
    public const string Polish = "pl";
    public const string English = "en";

    public static string Default => Polish;
    public static string[] List => [Polish, English];
}