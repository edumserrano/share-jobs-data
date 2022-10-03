namespace ShareJobsDataCli.Tests.Features.ReadDataCurrentWorkflow.DependencyErrors;

/// <summary>
/// These tests check what happens when the list artifacts HTTP dependency of the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class FailedHttpToListWorkflowRunArtifactsTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list the current workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError from the list workflow run artifact response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCode()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment);
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the response body contains some data vs when it's empty.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCodeWithBody()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment);
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task NullDeserialization()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContent("null");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment);
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to list workflow run artifacts fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the list workflow run artifact response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Theory]
    [InlineData("response-model-validation", "list-artifacts")]
    [InlineData("artifact-container-model-validation", "list-artifacts-2")]
    public async Task JsonModelValidation(string scenario, string listArtifactsResponseScenario)
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath($"{listArtifactsResponseScenario}.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment);
        using var console = new FakeInMemoryConsole();
        var exception = await Should.ThrowAsync<CommandException>(command.ExecuteAsync(console).AsTask());

        console.ReadOutputString().ShouldBeEmpty();
        await Verify(exception.Message)
            .AppendToMethodName("console-output")
            .UseParameters(scenario);
    }
}
