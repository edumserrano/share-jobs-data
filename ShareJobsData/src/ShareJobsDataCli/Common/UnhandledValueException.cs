namespace ShareJobsDataCli.Common;

public sealed class UnhandledValueException : Exception
{
    internal UnhandledValueException(object value, [CallerArgumentExpression("value")] string expression = "")
        : base($"Unhandled '{value.GetType().Name}' type for variable {expression}.")
    {
    }
}
