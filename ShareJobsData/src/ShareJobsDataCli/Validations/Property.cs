namespace ShareJobsDataCli.Validations;

internal static class Property
{
    internal static string NotNullOrWhiteSpace<TModel>([NotNull] this string? value, string propertyName)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var message = $"{propertyName} cannot be null or whitespace.";
        throw new PropertValidationException(typeof(TModel).Name, message);
    }

    internal static string ValidUri<TModel>([NotNull] this string? value, string propertyName)
    {
        var options = new UriCreationOptions();
        if(Uri.TryCreate(value, options, out var _))
        {
            return value;
        }

        var message = $"{propertyName} must be a valid URI. Received: '{value}'.";
        throw new PropertValidationException(typeof(TModel).Name, message);
    }

    public static int Positive<TModel>(this int value,  string propertyName)
    {
        if (value >= 0)
        {
            return value;
        }

        var message = $"{propertyName} must be a positive value. Received: '{value}'.";
        throw new PropertValidationException(typeof(TModel).Name, message); 
    }
}
