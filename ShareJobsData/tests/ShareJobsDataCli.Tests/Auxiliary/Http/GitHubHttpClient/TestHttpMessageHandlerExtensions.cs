namespace ShareJobsDataCli.Tests.Auxiliary.Http.GitHubHttpClient;

internal static class TestHttpMessageHandlerExtensions
{
    public static HttpResponseMessageMockBuilder WhereRequestUriEquals(this HttpResponseMessageMockBuilder builder, string path)
    {
        return builder.Where(httpRequestMessage =>
        {
            return httpRequestMessage.RequestUri!.ToString().Equals(path, StringComparison.OrdinalIgnoreCase);
        });
    }

    private static TestHttpMessageHandler MockHttpResponseWithCustomBuilder<TCustomHttpMockBuilder>(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<TCustomHttpMockBuilder> configure)
            where TCustomHttpMockBuilder : BaseResponseMockBuilder<TCustomHttpMockBuilder>, new()
    {
        return testHttpMessageHandler.MockHttpResponse(httpResponseBuilder =>
        {
            var customHttpMockBuilder = new TCustomHttpMockBuilder();
            configure(customHttpMockBuilder);
            customHttpMockBuilder.Build(httpResponseBuilder);
        });
    }

    public static TestHttpMessageHandler MockListArtifactsFromDifferentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<ListArtifactsFromDifferentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockDownloadArtifactFromDifferentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<DownloadArtifactFromDifferentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockListArtifactsFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<ListArtifactsFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockGetContainerItemsFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<GetContainerItemsFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockDownloadArtifactFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<DownloadArtifactFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockCreateArtifactContainerFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<CreateArtifactContainerFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockUploadArtifactFileFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<UploadArtifactFileFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }

    public static TestHttpMessageHandler MockFinalizeArtifactContainerFromCurrentWorkflowRun(
        this TestHttpMessageHandler testHttpMessageHandler,
        Action<FinalizeArtifactContainerFromCurrentWorkflowRunResponseMockBuilder> configure)
    {
        return testHttpMessageHandler.MockHttpResponseWithCustomBuilder(configure);
    }
}
