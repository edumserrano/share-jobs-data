namespace ShareJobsDataCli.CliCommands.Errors;

internal static class JsonHttpResultErrorExtensions
{
    public static string AsErrorMessage<T>(
        this JsonHttpResult<T>.Error jsonHttpResult,
        string action,
        string httpRequest)
            where T : class
    {
        return jsonHttpResult switch
        {
            JsonHttpResult<T>.JsonDeserializedToNull => GetErrorMessage(action, httpRequest),
            JsonHttpResult<T>.JsonModelValidationFailed failedValidation => GetErrorMessage(failedValidation, action, httpRequest),
            JsonHttpResult<T>.FailedStatusCode failedStatusCode => GetErrorMessage(failedStatusCode, action, httpRequest),
            _ => throw UnexpectedTypeException.Create(jsonHttpResult),
        };
    }

    private static string GetErrorMessage(string action, string httpRequest)
    {
        return $"Failed to {action} because the response from {httpRequest} deserialized to null.";
    }

    private static string GetErrorMessage<T>(
        JsonHttpResult<T>.FailedStatusCode failedStatusCode,
        string action,
        string httpRequest)
            where T : class
    {
        var httpErrorMessage = failedStatusCode.FailedStatusCodeHttpResponse.AsHttpErrorMessage();
        return $"Failed to {action} because the response from {httpRequest} returned an error status code. {httpErrorMessage}";
    }

    private static string GetErrorMessage<T>(
        JsonHttpResult<T>.JsonModelValidationFailed failedValidation,
        string action,
        string httpRequest)
            where T : class
    {
        var sb = new StringBuilder();
        for (var i = 0; i < failedValidation.ValidationResult.Errors.Count; i++)
        {
            var validationError = failedValidation.ValidationResult.Errors[i];
            sb.Append(i + 1).Append(") ").AppendLine(validationError.ToString());
        }

        var validationErrors = sb.ToString();
        return $"Failed to {action} because the response from {httpRequest} failed validation rules:{Environment.NewLine}{validationErrors}";
    }
}
