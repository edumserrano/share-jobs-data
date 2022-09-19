namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> outputs the value to the console.
/// </summary>
[Trait("Category", XUnitCategories.Commands)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandOkTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> downloads the workflow artifact with the shared
    /// job data and outputs it to the console as a GitHub step output.
    /// </summary>
    [Fact]
    public async Task Success()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("list-artifacts.http-response.json"));
        });
        testHttpMessageHandler.MockDownloadArtifactFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowArtifactId(repoName, artifactId: "351670722")
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("download-artifact.http-response.zip"));
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
            ArtifactName = "job-data",
            ArtifactFilename = "job-data.json",
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadOutputString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
