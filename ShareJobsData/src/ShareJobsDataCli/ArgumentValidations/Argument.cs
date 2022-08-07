namespace ShareJobsDataCli.ArgumentValidations;

internal static class Argument
{
    public static T NotNull<T>([NotNull] this T? value, [CallerArgumentExpression("value")] string name = "")
        where T : class
    {
        return value ?? throw new ArgumentNullException(name);
    }

    internal static string NotNullOrWhiteSpace([NotNull] this string? value, [CallerArgumentExpression("value")] string name = "")
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var message = $"{name} cannot be null or whitespace.";
        throw new ArgumentException(message);
    }

    public static int Positive(this int value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value >= 0)
        {
            return value;
        }

        var message = $"{name} must be a positive value. Received '{value}'.";
        throw new ArgumentException(message);
    }

    public static long Positive(this string value, [CallerArgumentExpression("value")] string name = "")
    {
        if (long.TryParse(value, out var parsed) && parsed >= 0)
        {
            return parsed;
        }

        var message = $"{name} must be a positive value. Received '{value}'.";
        throw new ArgumentException(message);
    }
}
