namespace ShareJobsDataCli.Validations;

internal static class Property
{
    internal static string PropertyNotNullOrWhiteSpace<TModel>([NotNull] this string? value, string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var message = $"{propertyName} cannot be null or whitespace.";
        throw new PropertValidationException(typeof(TModel).Name, message);
    }

    //public static int PropertyPositive(this int value, [CallerArgumentExpression("value")] string expression = "")
    //{
    //    if (value >= 0)
    //    {
    //        return value;
    //    }

    //    var message = $"{expression} must be a positive value. Received '{value}'.";
    //    throw new PropertValidationException(message);
    //}
}

public class PropertValidationException : Exception
{
    internal PropertValidationException(string modelName, string validationErrorMessage)
        : base($"Error validating {modelName}: {validationErrorMessage}")
    {
    }
}
