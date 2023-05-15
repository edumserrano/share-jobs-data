namespace ShareJobsDataCli.Common.Guards;

public sealed class UnexpectedTypeException : Exception
{
    internal UnexpectedTypeException(string message)
        : base(message)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static UnexpectedTypeException Create(object value, [CallerArgumentExpression(nameof(value))] string expression = "")
    {
        var type = value.GetType();
        var message = $"{expression} represented an unhandled type: '{type.Name}' in '{type.Namespace}'.";
        return new UnexpectedTypeException(message);
    }
}
