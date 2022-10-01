namespace ShareJobsDataCli.Tests.Features.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> outputs the value to the console.
/// </summary>
[Trait("Category", XUnitCategories.Commands)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class ReadDataFromDifferentGitHubWorkflowCommandOkTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> downloads the workflow artifact with the shared
    /// job data and outputs it to the console using different output options.
    /// </summary>
    [Theory]
    [InlineData("strict-json")]
    [InlineData("github-step-json")]
    public async Task Success(string outputOption)
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("list-artifacts.http-response.json"));
        });
        testHttpMessageHandler.MockDownloadArtifactFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowArtifactId(repoName, artifactId: "351670722")
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("download-artifact.http-response.zip"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
            ArtifactName = "job-data",
            ArtifactFilename = "job-data.json",
            Output = outputOption,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadErrorString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output)
            .AppendToMethodName("console-output")
            .UseParameters(outputOption);
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> makes the expected HTTP requests when downloading a workflow artifact.
    /// </summary>
    [Fact]
    public async Task MakesExpectedHttpRequests()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("list-artifacts.http-response.json"));
        });
        testHttpMessageHandler.MockDownloadArtifactFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowArtifactId(repoName, artifactId: "351670722")
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("download-artifact.http-response.zip"));
        });
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);
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
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
