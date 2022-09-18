namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests check what happens when a dependency of the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandDependencyErrorTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError from the list workflow run artifact response.
    /// </summary>
    [Fact]
    public async Task FailedHttpToListWorkflowRunArtifacts()
    {
        const string repoName = "test-repo-name";
        const string runId = "test-run-id";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(httpRequestMessage => new HttpResponseMessage(HttpStatusCode.InternalServerError) { RequestMessage = httpRequestMessage })
            .Build();
        using var testHandler = new TestHttpMessageHandler();
        testHandler.MockHttpResponse(listArtifactsHttpMock);
        using var outgoingHttpCallsHandler = new RecordingHandler
        {
            InnerHandler = testHandler,
        };
        using var httpClient = new HttpClient(outgoingHttpCallsHandler);
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts)}.outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to download the artifact fails.
    /// Simulating an HttpStatusCode.InternalServerError from the download artifact response.
    /// </summary>
    [Fact]
    public async Task FailedHttpToDownloadArtifact()
    {
        const string repoName = "test-repo-name";
        const string runId = "test-run-id";
        const string baseRelativeFilepath = $"tests/ShareJobsDataCli.Tests/CliCommands/ReadDataDifferentWorkflow/{nameof(ReadDataFromDifferentGitHubWorkflowCommandDependencyErrorTests)}.{nameof(FailedHttpToDownloadArtifact)}";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(_ =>
            {
                const string listArtifactsResponseRelativePath = $"{baseRelativeFilepath}.list-artifacts.http-response.json";
                var listArtifactsResponseFilepath = TestsProj.GetAbsoluteFilepath(listArtifactsResponseRelativePath);
                var responseContent = File.ReadAllText(listArtifactsResponseFilepath);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent),
                };
            })
            .Build();
        var downloadArtifactHttpCallMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.PathAndQuery.Equals("/repos/edumserrano/share-jobs-data/actions/artifacts/351670722/zip", StringComparison.OrdinalIgnoreCase))
            .RespondWith(httpRequestMessage => new HttpResponseMessage(HttpStatusCode.InternalServerError) { RequestMessage = httpRequestMessage })
            .Build();
        using var testHandler = new TestHttpMessageHandler();
        testHandler.MockHttpResponse(listArtifactsHttpMock);
        testHandler.MockHttpResponse(downloadArtifactHttpCallMock);
        using var outgoingHttpCallsHandler = new RecordingHandler
        {
            InnerHandler = testHandler,
        };
        using var httpClient = new HttpClient(outgoingHttpCallsHandler);
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
            ArtifactName = "job-data",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(FailedHttpToDownloadArtifact)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(FailedHttpToDownloadArtifact)}.outbound-http");
    }
}
