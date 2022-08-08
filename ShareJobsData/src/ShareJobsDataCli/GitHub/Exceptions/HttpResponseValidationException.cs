namespace ShareJobsDataCli.GitHub.Exceptions;

public sealed class HttpResponseValidationException : Exception
{
    internal HttpResponseValidationException(string message)
        : base(message)
    {
    }

    internal static HttpResponseValidationException JsonDeserializedToNull<T>()
    {
        return new HttpResponseValidationException($"{typeof(T)} deserialized to null.");
    }

    internal static HttpResponseValidationException ValidationFailed<T>(ValidationResult validationResult)
    {
        var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage);
        var formattedErrorMessages = string.Join($"{Environment.NewLine}  - ", errorMessages);
        var message = $"Failed to validate {typeof(T)}:{Environment.NewLine}  - {formattedErrorMessages}";
        return new HttpResponseValidationException(message);
    }
}
