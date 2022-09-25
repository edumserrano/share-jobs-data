namespace ShareJobsDataCli.CliCommands.Errors;

internal static class ErrorDetailsExtensions
{
    public static string ToErrorDetails<T>(this JsonHttpResult<T>.Error jsonHttpResult, string operationName)
        where T : class
    {
        return jsonHttpResult switch
        {
            JsonHttpResult<T>.JsonDeserializedToNull => $"HTTP response from '{operationName}' deserialized to null.",
            JsonHttpResult<T>.JsonModelValidationFailed failedValidation => $"HTTP response from '{operationName}' validation.{Environment.NewLine}{failedValidation.ValidationResult}",
            JsonHttpResult<T>.FailedStatusCode failedStatusCode => $"HTTP response from '{operationName}' returned error status code.{Environment.NewLine}{failedStatusCode.FailedStatusCodeHttpResponse.FormatFailedHttpResponseMessage()}",
            _ => throw UnexpectedTypeException.Create(jsonHttpResult),
        };
    }

    public static string ToErrorDetails(this FailedStatusCodeHttpResponse failedStatusCodeHttpResponse, string operationName)
    {
        return $"HTTP response from {operationName} returned error status code.{Environment.NewLine}{failedStatusCodeHttpResponse.FormatFailedHttpResponseMessage()}";
    }

    public static string ToErrorDetails(this GitHubArtifactItemNotJsonContent notJsonContent)
    {
        var artifactItemContent = string.IsNullOrEmpty(notJsonContent.ItemContent)
            ? "<empty>"
            : $"{Environment.NewLine}---START---{Environment.NewLine}{notJsonContent.ItemContent}{Environment.NewLine}---END---";
        return @$"Content from downloaded artifact item must be JSON.
Artifact item content:
{artifactItemContent}
JSON error:
{notJsonContent.JsonReaderErrorMessage}";
    }

    private static string FormatFailedHttpResponseMessage(this FailedStatusCodeHttpResponse httpResponse)
    {
        var responseBody = string.IsNullOrEmpty(httpResponse.ResponseBody)
            ? "<empty>"
            : $"{Environment.NewLine}---START---{Environment.NewLine}{httpResponse.ResponseBody}{Environment.NewLine}---END---";
        return @$"HTTP Method: {httpResponse.Method}
Request URL: {httpResponse.RequestUrl}
Response status code: {httpResponse.StatusCode}
Response body: {responseBody}";
    }


}
