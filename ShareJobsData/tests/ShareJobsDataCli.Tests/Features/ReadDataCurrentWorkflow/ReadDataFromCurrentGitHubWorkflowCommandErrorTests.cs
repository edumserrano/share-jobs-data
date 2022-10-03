namespace ShareJobsDataCli.Tests.Features.ReadDataCurrentWorkflow;

/// <summary>
/// These tests check what happens when a logic violation occurs when running the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.LogicFailure)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class ReadDataFromCurrentGitHubWorkflowCommandErrorTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when it can't download
    /// the specified artifact because it doesn't exist on the current workflow run.
    /// </summary>
    [Fact]
    public async Task ArtifactNotFound()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("list-artifacts.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = "not-gonna-find-it",
            ArtifactFilename = "job-data.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when it can't download
    /// the specified artifact file because it doesn't exist on the downloaded artifact from the current workflow run.
    /// </summary>
    [Fact]
    public async Task ArtifactFileNotFound()
    {
        const string artifactName = "job-data";
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("list-artifacts.http-response.json"));
        });
        testHttpMessageHandler.MockGetContainerItemsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromFileContainerResourceUrl(
                    fileContainerResourceUrl: "https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982",
                    artifactName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("get-container-items.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = "not-gonna-find-me.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when the --output is invalid.
    /// </summary>
    [Fact]
    public async Task InvalidOutput()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            Output = "not-valid-value",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        outboundHttpRequests.ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }
}
