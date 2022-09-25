namespace ShareJobsDataCli.Tests.CliCommands.ReadDataCurrentWorkflow;

/// <summary>
/// These tests make sure that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> outputs the value to the console.
/// </summary>
[Trait("Category", XUnitCategories.Commands)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromCurrentGitHubWorkflowCommandOkTests
{
    /// <summary>
    /// Tests that the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> downloads the workflow artifact with the shared
    /// job data and outputs it to the console as a GitHub step output.
    /// </summary>
    [Fact]
    public async Task Success()
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
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("download-artifact.http-response.json"));
        });
        (var httpClient, var outboundHttpRequests) = TestHttpClient.Create(testHttpMessageHandler);
        httpClient.BaseAddress = new Uri("https://test-base-address.com");

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
}
