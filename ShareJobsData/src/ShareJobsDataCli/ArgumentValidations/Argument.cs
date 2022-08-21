namespace ShareJobsDataCli.ArgumentValidations;

internal static class Argument
{
    public static T NotNull<T>([NotNull] this T? value, [CallerArgumentExpression("value")] string expression = "")
        where T : class
    {
        return value ?? throw new ArgumentNullException(expression);
    }

    internal static string NotNullOrWhiteSpace([NotNull] this string? value, [CallerArgumentExpression("value")] string expression = "")
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var message = $"{expression} cannot be null or whitespace.";
        throw new ArgumentException(message);
    }

    public static long Positive(this long value, [CallerArgumentExpression("value")] string expression = "")
    {
        if (value >= 0)
        {
            return value;
        }

        var message = $"{expression} must be a positive value. Received '{value}'.";
        throw new ArgumentException(message);
    }
}
