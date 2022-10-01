namespace ShareJobsDataCli.Guards;

// Using part of the idea of a ThrowHelper pattern due to the benefits explained here: https://dunnhq.com/posts/2022/throw-helper/
// See also:
// https://github.com/dotnet/runtime/blob/9e81ca53137c587bce0f30bf60f13bb10fbdd204/src/libraries/System.Private.CoreLib/src/System/ArgumentNullException.cs#L63
// https://github.com/dotnet/runtime/blob/9e81ca53137c587bce0f30bf60f13bb10fbdd204/src/libraries/System.Private.CoreLib/src/System/ArgumentException.cs#L110
// https://github.com/dotnet/runtime/blob/953f52482ac2460e4b3faff33e4f73c9b30cd7b4/src/libraries/System.Memory/src/System/ThrowHelper.cs
//
// In short:
// "Having a throw new in your methods can be inefficient. The inefficiency comes from the fact that a fair amount of assembly code is
// generated to throw the exception."
// "... there is a pattern that can help both eliminate the extra assembly code instructions, and optimize the code so that registers
// are not set-up needlessly."
//
// Can't throw the exception from the methods because I don't use this exception for validation purposes at the start of methods,
// I use it to short circuit execution flow.
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
