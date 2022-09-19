namespace ShareJobsDataCli.Tests.Auxiliary.Http;

internal static class TestHttpClientFactory
{
    public static (HttpClient httpClient, OutboundHttpRequests outboundHttpRequests) Create(TestHttpMessageHandler testHttpMessageHandler)
    {
#pragma warning disable CA2000 // Dispose objects before losing scope
        var recordingHandler = new RecordingHandler
        {
            InnerHandler = testHttpMessageHandler,
        };
#pragma warning restore CA2000 // Dispose objects before losing scope
        var httpClient = new HttpClient(recordingHandler);
        var outboundHttpRequests = new OutboundHttpRequests(recordingHandler.Sends);
        return (httpClient, outboundHttpRequests);
    }
}
