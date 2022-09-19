namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

internal static class TestHttpMessageHandlerExtensions
{
    public static HttpResponseMessageMockBuilder WhereRequestPathEquals(this HttpResponseMessageMockBuilder builder, string path)
    {
        return builder.Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals(path, StringComparison.OrdinalIgnoreCase));
    }

    public static TestHttpMessageHandler MockListArtifactsFromDifferentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<ListArtifactsFromDifferentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponse(builder =>
        {
            var listArtifactsResponseMockBuilder = new ListArtifactsFromDifferentWorkflowRunResponseMockBuilder();
            configure(listArtifactsResponseMockBuilder);
            listArtifactsResponseMockBuilder.Build(builder);
        });
    }

    public static TestHttpMessageHandler MockDownloadArtifactFromDifferentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponse(builder =>
        {
            var downloadArtifactResponseMockBuilder = new DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder();
            configure(downloadArtifactResponseMockBuilder);
            downloadArtifactResponseMockBuilder.Build(builder);
        });
    }
}
