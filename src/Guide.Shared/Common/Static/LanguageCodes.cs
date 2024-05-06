namespace Guide.Shared.Common.Static;

public static class LanguageCodes
{
    public const string Polish = "pl-PL";
    public const string English = "en-US";

    public static string Default => Polish;
    public static string[] List => [Polish, English];
}