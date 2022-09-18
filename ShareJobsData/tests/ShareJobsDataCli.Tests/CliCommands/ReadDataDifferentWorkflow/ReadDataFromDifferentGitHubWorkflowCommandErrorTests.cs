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
        const string repoName = "test-repo-name";
        const string runId = "test-run-id";
        const string baseRelativeFilepath = $"tests/ShareJobsDataCli.Tests/CliCommands/ReadDataDifferentWorkflow/{nameof(ReadDataFromDifferentGitHubWorkflowCommandErrorTests)}.{nameof(ArtifactNotFound)}";
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
            ArtifactName = "not-gonna-find-it",
            ArtifactFilename = "job-data.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(ArtifactNotFound)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(ArtifactNotFound)}.outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it can't download
    /// the specified artifact file because it doesn't exist on the downloaded artifact.
    /// </summary>
    [Fact]
    public async Task ArtifactFileNotFound()
    {
        const string repoName = "test-repo-name";
        const string runId = "test-run-id";
        const string baseRelativeFilepath = $"tests/ShareJobsDataCli.Tests/CliCommands/ReadDataDifferentWorkflow/{nameof(ReadDataFromDifferentGitHubWorkflowCommandErrorTests)}.{nameof(ArtifactFileNotFound)}";
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
            .RespondWith(_ =>
            {
                const string downloadArtifactResponseRelativePath = $"{baseRelativeFilepath}.download-artifact.http-response.zip";
                var downloadArtifactResponseFilepath = TestsProj.GetAbsoluteFilepath(downloadArtifactResponseRelativePath);
                var fileStream = File.Open(downloadArtifactResponseFilepath, FileMode.Open);
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(fileStream),
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Zip);
                return response;
            })
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
            ArtifactFilename = "not-gonna-find-me.json",
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(ArtifactFileNotFound)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(ArtifactFileNotFound)}.outbound-http");
    }
}
