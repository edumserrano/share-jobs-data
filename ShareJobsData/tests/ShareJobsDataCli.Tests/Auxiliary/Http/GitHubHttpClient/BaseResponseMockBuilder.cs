namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

internal abstract class BaseResponseMockBuilder<T>
    where T : BaseResponseMockBuilder<T>
{
    protected HttpStatusCode ResponseHttpStatusCode { get; set; } = HttpStatusCode.OK;

    protected TestFilepath? ResponseContentFilepath { get; set; }

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

    public abstract void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder);
}
