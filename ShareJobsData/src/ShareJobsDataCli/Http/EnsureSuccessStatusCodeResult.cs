namespace ShareJobsDataCli.Http;

internal abstract record EnsureSuccessStatusCodeResult
{
    private EnsureSuccessStatusCodeResult()
    {
    }

    public sealed record Ok
        : EnsureSuccessStatusCodeResult;

    public sealed record NonSuccessStatusCode(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : EnsureSuccessStatusCodeResult;

    public static implicit operator EnsureSuccessStatusCodeResult(FailedStatusCodeHttpResponse failedStatusCodeHttpResponse) => new NonSuccessStatusCode(failedStatusCodeHttpResponse);

    public bool IsOk([NotNullWhen(returnValue: false)] out FailedStatusCodeHttpResponse? failedStatusCodeHttpResponse)
    {
        if (this is NonSuccessStatusCode nonSuccessStatusCode)
        {
            failedStatusCodeHttpResponse = nonSuccessStatusCode.FailedStatusCodeHttpResponse;
            return false;
        }

        failedStatusCodeHttpResponse = null;
        return true;
    }
}

internal sealed record FailedStatusCodeHttpResponse(
    string Method,
    string RequestUrl,
    string StatusCode,
    string ResponseBody);

internal static class FailedStatusCodeHttpResponseExtensions
{
    public static FailedStatusCodeHttpResponse ToFailedStatusCodeHttpResponse(this HttpResponseMessage httpResponse, string responseBody)
    {
        httpResponse.NotNull();

        var method = httpResponse.RequestMessage?.Method.ToString() ?? "Unknown";
        var url = httpResponse.RequestMessage?.RequestUri?.ToString() ?? "Unknown";
        var statusCode = httpResponse.StatusCode.ToString();
        return new FailedStatusCodeHttpResponse(method, url, statusCode, responseBody);
    }
}
