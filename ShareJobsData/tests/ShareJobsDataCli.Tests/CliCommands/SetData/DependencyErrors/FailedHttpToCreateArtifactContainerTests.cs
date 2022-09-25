namespace ShareJobsDataCli.Tests.CliCommands.SetData.DependencyErrors;

/// <summary>
/// These tests check what happens when the create artifact container HTTP dependency of the <see cref="SetDataCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public class FailedHttpToCreateArtifactContainerTests
{
    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the HTTP request to
    /// create the artifact container fails.
    /// Simulating an HttpStatusCode.InternalServerError from the create artifact container response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCode()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error when the HTTP request to
    /// create the artifact container fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the create artifact container response.
    /// This allows testing the formatting of the error message on the output when the response body contains some data vs when it's empty.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCodeWithBody()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error when the HTTP request to
    /// create the artifact container fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the create artifact container response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task NullDeserialization()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContent("null");
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> hows expected error when the HTTP request to
    /// create the artifact container fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the create artifact container response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Fact]
    public async Task JsonModelValidation()
    {
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("create-artifact-container.http-response.json"));
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            DataAsYmlStr = TestFiles.GetFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
