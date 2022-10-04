namespace ShareJobsDataCli.Common.Guards;

// See /docs/dev-notes/code-details/throw-helper.md to understand more about this implementation.
public sealed class UnexpectedTypeException : Exception
{
    internal UnexpectedTypeException(string message)
        : base(message)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static UnexpectedTypeException Create(object value, [CallerArgumentExpression("value")] string expression = "")
    {
        var type = value.GetType();
        var message = $"{expression} represented an unhandled type: '{type.Name}' in '{type.Namespace}'.";
        return new UnexpectedTypeException(message);
    }
}
