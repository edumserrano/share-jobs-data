namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow.DependencyErrors;

/// <summary>
/// These tests check what happens when the download artifact item HTTP dependency of the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class FailedHttpToListWorkflowRunArtifactsTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError from the list workflow run artifact response.
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
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClient.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the response body contains some data vs when it's empty.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCodeWithBody()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClient.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task NullDeserialization()
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContent("null");
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClient.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Theory]
    [InlineData("response-model-validation", "list-artifacts")]
    [InlineData("artifact-model-validation", "list-artifacts-2")]
    public async Task JsonModelValidation(string scenario, string listArtifactsResponseScenario)
    {
        const string repoName = "edumserrano/share-jobs-data";
        const string runId = "test-run-id";
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromDifferentWorkflowRun(builder =>
        {
            builder
                .FromWorkflowRun(repoName, runId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath($"{listArtifactsResponseScenario}.http-response.json"));
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClient.Create(testHttpMessageHandler);
        var githubEnvironment = new TestsGitHubEnvironment();

        var command = new ReadDataFromDifferentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repoName,
            RunId = runId,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output)
            .AppendToMethodName("console-output")
            .UseParameters(scenario);
        await Verify(outboundHttpRequests)
            .AppendToMethodName("outbound-http")
            .UseParameters(scenario);
    }
}
