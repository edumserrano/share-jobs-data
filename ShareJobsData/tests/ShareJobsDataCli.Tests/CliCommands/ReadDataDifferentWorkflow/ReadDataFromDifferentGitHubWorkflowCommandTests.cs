namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> outputs the value to the console.
/// </summary>
[Trait("Category", XUnitCategories.Commands)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> downloads the workflow artifact with the shared
    /// job data and outputs it to the console as a GitHub step output.
    /// </summary>
    [Fact]
    public async Task Success()
    {
        const string repoName = "test-repo-name";
        const string runId = "test-run-id";
        const string baseRelativeFilepath = $"tests/ShareJobsDataCli.Tests/CliCommands/ReadDataDifferentWorkflow/{nameof(ReadDataFromDifferentGitHubWorkflowCommandTests)}.{nameof(Success)}.http-response";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(_ =>
            {
                const string listArtifactsResponseRelativePath = $"{baseRelativeFilepath}.list-artifacts.json";
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
                const string downloadArtifactResponseRelativePath = $"{baseRelativeFilepath}.download-artifact.zip";
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
            ArtifactFilename = "job-data.json",
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);
        var output = console.ReadOutputString();

        await Verify(output).UseMethodName($"{nameof(Success)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(Success)}.http-calls");
    }
}
