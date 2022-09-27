namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow.DependencyErrors;

/// <summary>
/// These tests check what happens when the list artifacts HTTP dependency of the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class FailedHttpToDownloadArtifactTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to download the artifact fails.
    /// Simulating an HttpStatusCode.InternalServerError from the download artifact response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCode()
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
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
            ArtifactName = "job-data",
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when the
    /// content of the downloaded artifact is not JSON.
    /// </summary>
    [Fact]
    public async Task ArtifactContentIsNotJson()
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
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("download-artifact.http-response.zip"));
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
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }
}
