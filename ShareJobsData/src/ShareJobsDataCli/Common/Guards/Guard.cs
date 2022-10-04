namespace ShareJobsDataCli.Common.Guards;

// See /docs/dev-notes/code-details/throw-helper.md to understand more about this implementation.
internal static class Guard
{
    public static T NotNull<T>([NotNull] this T? value, [CallerArgumentExpression("value")] string expression = "")
    {
        if (value is null)
        {
            GuardException.Throw($"{expression} cannot be null.");
        }

        return value;
    }

    public static string NotNullOrWhiteSpace([NotNull] this string? value, [CallerArgumentExpression("value")] string expression = "")
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            GuardException.Throw($"{expression} cannot be null or whitespace.");
        }

        return value;
    }

    public static long Positive(this long value, [CallerArgumentExpression("value")] string expression = "")
    {
        if (value < 0)
        {
            GuardException.Throw($"{expression} must be a positive value. Received '{value.ToString(CultureInfo.InvariantCulture)}'.");
        }

        return value;
    }
}
