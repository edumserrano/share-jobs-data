namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

internal abstract class BaseResponseMockBuilder<T>
    where T : BaseResponseMockBuilder<T>
{
    protected HttpStatusCode ResponseHttpStatusCode { get; set; } = HttpStatusCode.OK;

    protected TestFilepath? ResponseContentFilepath { get; set; }

    protected string? ResponseContentAsString { get; set; }

    protected abstract string OperationName { get; }

    public T WithResponseStatusCode(HttpStatusCode responseHttpStatusCode)
    {
        ResponseHttpStatusCode = responseHttpStatusCode;
        return (T)this;
    }

    public T WithResponseContentFromFilepath(TestFilepath responseContentFilepath)
    {
        ResponseContentFilepath = responseContentFilepath;
        return (T)this;
    }

    public T WithResponseContent(string responseContent)
    {
        ResponseContentAsString = responseContent;
        return (T)this;
    }

    protected abstract string GetRequestUrl();

    protected virtual HttpContent? GetResponseContent()
    {
        if (ResponseContentFilepath is not null)
        {
            return ResponseContentFilepath.ReadFileAsStringContent();
        }

        if (ResponseContentAsString is not null)
        {
            return new StringContent(ResponseContentAsString);
        }

        return null;
    }

    public void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder)
    {
        var requestUrl = GetRequestUrl();
        if (string.IsNullOrEmpty(requestUrl))
        {
            throw new InvalidOperationException($"Invalid response mock configuration for {OperationName}");
        }

        var responseContent = GetResponseContent();
        httpResponseMessageMockBuilder
            .WhereRequestUriEquals(requestUrl)
            .RespondWith(httpRequestMessage => new HttpResponseMessage(ResponseHttpStatusCode)
            {
                // this is required when testing failure status codes because the app returns information from the http request made
                RequestMessage = httpRequestMessage,
                Content = responseContent,
            });
    }
}
