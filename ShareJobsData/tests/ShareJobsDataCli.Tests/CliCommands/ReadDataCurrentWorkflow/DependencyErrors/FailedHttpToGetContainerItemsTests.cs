namespace ShareJobsDataCli.Tests.CliCommands.ReadDataCurrentWorkflow.DependencyErrors;

/// <summary>
/// These tests check what happens when the get container items HTTP dependency of the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class FailedHttpToGetContainerItemsTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to get container items fails.
    /// Simulating an HttpStatusCode.InternalServerError from the get container items response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCode()
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
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to get container items fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the get container items response.
    /// This allows testing the formatting of the error message on the output when the response body contains some data vs when it's empty.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCodeWithBody()
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
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to get container items fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the get container items response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task NullDeserialization()
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
                .WithResponseContent("null");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to get container items fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the get container items response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Theory]
    [InlineData("response-model-validation", "get-container-items")]
    [InlineData("artifact-container-item-model-validation", "get-container-items-2")]
    public async Task JsonModelValidation(string scenario, string getContainerItemsResponseScenario)
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
                .WithResponseContentFromFilepath(TestFiles.GetFilepath($"{getContainerItemsResponseScenario}.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output)
            .AppendToMethodName("console-output")
            .UseParameters(scenario);
    }
}
