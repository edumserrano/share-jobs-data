namespace ShareJobsDataCli.GitHub;

internal abstract record EnsureSuccessStatusCodeResult
{
    private EnsureSuccessStatusCodeResult()
    {
    }

    public record Ok()
        : EnsureSuccessStatusCodeResult;

    public record NonSuccessStatusCode(FailedStatusCodeHttpResponse FailedStatusCodeHttpResponse)
        : EnsureSuccessStatusCodeResult;

    public static implicit operator EnsureSuccessStatusCodeResult(FailedStatusCodeHttpResponse failedStatusCodeHttpResponse) => new NonSuccessStatusCode(failedStatusCodeHttpResponse);

    public static implicit operator EnsureSuccessStatusCodeResult(NoneResult _) => new Ok();
}

public record FailedStatusCodeHttpResponse
(
    string Method,
    string RequestUrl,
    string StatusCode,
    string ResponseBody
);
