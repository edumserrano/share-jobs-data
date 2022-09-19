namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests check what happens when a logic violation occurs when running the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.LogicFailure)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandErrorTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it can't download
    /// the specified artifact because it doesn't exist on the specified workflow run id.
    /// </summary>
    [Fact]
    public async Task ArtifactNotFound()
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
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
            ArtifactName = "not-gonna-find-it",
            ArtifactFilename = "job-data.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it can't download
    /// the specified artifact file because it doesn't exist on the downloaded artifact.
    /// </summary>
    [Fact]
    public async Task ArtifactFileNotFound()
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
            ArtifactFilename = "not-gonna-find-me.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
