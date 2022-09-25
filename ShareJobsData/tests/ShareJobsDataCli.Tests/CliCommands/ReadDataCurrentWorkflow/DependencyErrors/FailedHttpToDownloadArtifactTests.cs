namespace ShareJobsDataCli.Tests.CliCommands.ReadDataCurrentWorkflow.DependencyErrors;

/// <summary>
/// These tests check what happens when the download artifact item HTTP dependency of the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public class FailedHttpToDownloadArtifactTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when 
    /// the HTTP request to download artifact item fails.
    /// Simulating an HttpStatusCode.InternalServerError from the download artifact item response.
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
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("list-artifacts.http-response.json"));
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
        testHttpMessageHandler.MockDownloadArtifactFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromContainerItemLocation("https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982?itemPath=job-data%2Fjob-data.json")
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when
    /// the HTTP request to download artifact item fails.
    /// Simulating an HttpStatusCode.InternalServerError with some error body from the download artifact item response.
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
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("list-artifacts.http-response.json"));
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
        testHttpMessageHandler.MockDownloadArtifactFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromContainerItemLocation("https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982?itemPath=job-data%2Fjob-data.json")
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }

    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> shows expected error message when the
    /// content of the downloaded artifact is not JSON.
    /// </summary>
    [Theory]
    [InlineData("empty-string", "")]
    [InlineData("white-spaces-string", "   ")]
    [InlineData("null-string", "null")]
    [InlineData("not-json", "{")]
    public async Task ArtifactContentIsNotJson(string scenario, string nonJsonArtifactContent)
    {
        const string artifactName = "job-data";
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockListArtifactsFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("list-artifacts.http-response.json"));
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
        testHttpMessageHandler.MockDownloadArtifactFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromContainerItemLocation("https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982?itemPath=job-data%2Fjob-data.json")
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContent(nonJsonArtifactContent);
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClientFactory.Create(testHttpMessageHandler);

        var command = new ReadDataFromCurrentGitHubWorkflowCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
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
