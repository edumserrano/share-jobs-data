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
    public async Task FailedHttpToListWorkflowRunArtifacts1()
    {
        const string repoName = "edumserrano/share-jobs-data";
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

        await Verify(exception.Message).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts1)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts1)}.outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the response body contains some data vs when it's empty.
    /// </summary>
    [Fact]
    public async Task FailedHttpToListWorkflowRunArtifacts2()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(httpRequestMessage =>
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Oops, something went wrong."),
                    RequestMessage = httpRequestMessage,
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
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts2)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts2)}.outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task FailedHttpToListWorkflowRunArtifacts3()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(httpRequestMessage =>
            {
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("null"),
                    RequestMessage = httpRequestMessage,
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
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts3)}.console-output");
        await Verify(outgoingHttpCallsHandler.Sends).UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts3)}.outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Theory]
    [InlineData("response-model-validation", "list-artifacts")]
    [InlineData("artifact-model-validation", "list-artifacts-2")]
    public async Task FailedHttpToListWorkflowRunArtifacts4(string scenario, string listArtifactsResponseScenario)
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(httpRequestMessage =>
            {
                var listArtifactsResponseFilepath = TestFiles.GetFilepath($"{listArtifactsResponseScenario}.http-response.json");
                var responseContent = File.ReadAllText(listArtifactsResponseFilepath);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent),
                    RequestMessage = httpRequestMessage,
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
        };
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(() => command.ExecuteAsync(console).AsTask());

        await Verify(exception.Message)
            .UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts4)}.console-output")
            .UseParameters(scenario);
        await Verify(outgoingHttpCallsHandler.Sends)
            .UseMethodName($"{nameof(FailedHttpToListWorkflowRunArtifacts4)}.outbound-http")
            .UseParameters(scenario);
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when it
    /// the HTTP request to download the artifact fails.
    /// Simulating an HttpStatusCode.InternalServerError from the download artifact response.
    /// </summary>
    [Fact]
    public async Task FailedHttpToDownloadArtifact()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        var listArtifactsHttpMock = new HttpResponseMessageMockBuilder()
            .Where(httpRequestMessage => httpRequestMessage.RequestUri!.AbsolutePath.Equals($"/repos/{repoName}/actions/runs/{runId}/artifacts", StringComparison.OrdinalIgnoreCase))
            .RespondWith(_ =>
            {
                var listArtifactsResponseFilepath = TestFiles.GetFilepath("list-artifacts.http-response.json");
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
