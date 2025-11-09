namespace GhostCore.Data.Evaluation.SourceGen;

public static class QuickExtensions
{
    public static string Capitalize(this string str)
    {
        return $"{char.ToUpper(str[1])}{str.Substring(2)}";
    }

    public static string Fieldify(this string str)
    {
        return $"_{char.ToLower(str[0])}{str.Substring(1)}";
    }
}