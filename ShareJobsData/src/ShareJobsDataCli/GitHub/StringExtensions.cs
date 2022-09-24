namespace ShareJobsDataCli.GitHub;

internal static class StringExtensions
{
    public static bool IsJson(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return false;
        }

        try
        {
            var jsonDocument = JsonDocument.Parse(source);
            return jsonDocument.RootElement.ValueKind is not JsonValueKind.Null;
        }
        catch (System.Text.Json.JsonException)
        {
            return false;
        }
    }
}
