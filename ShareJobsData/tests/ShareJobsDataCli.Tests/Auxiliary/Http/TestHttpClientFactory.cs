namespace ShareJobsDataCli.Tests.Auxiliary.Http;

internal static class TestHttpClient
{
    public static (HttpClient HttpClient, OutboundHttpRequests OutboundHttpRequests) CreateWithRecorder(TestHttpMessageHandler testHttpMessageHandler)
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
