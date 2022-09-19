namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

internal abstract class BaseResponseMockBuilder
{
    protected HttpStatusCode ResponseHttpStatusCode { get; set; } = HttpStatusCode.OK;

    protected TestFilepath? ResponseContentFilepath { get; set; }

    public BaseResponseMockBuilder WithResponseStatusCode(HttpStatusCode responseHttpStatusCode)
    {
        ResponseHttpStatusCode = responseHttpStatusCode;
        return this;
    }

    public BaseResponseMockBuilder WithResponseContentFromFilepath(TestFilepath responseContentFilepath)
    {
        ResponseContentFilepath = responseContentFilepath;
        return this;
    }

    public abstract void Build(HttpResponseMessageMockBuilder httpResponseMessageMockBuilder);
}
