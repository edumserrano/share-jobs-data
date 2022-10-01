namespace ShareJobsDataCli.Tests.Features.SetData;

/// <summary>
/// These tests make sure that the <see cref="SetDataCommand"/> outputs the value to the console.
/// </summary>
[Trait("Category", XUnitCategories.Commands)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public sealed class SetDataCommandOkTests
{
    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> uploads the specified YML data as a workflow artifact and sets
    /// the data as GitHub step output if requested.
    ///
    /// This tests different output methods as well as testing that multiline yaml data input are outputted as expected.
    /// Multiline yaml values should have the newline characters trimmed at the end.
    /// </summary>
    [Theory]
    [InlineData("no-output", "none", "job-data.input.yml")]
    [InlineData("strict-json", "strict-json", "job-data.input.yml")]
    [InlineData("github-step-json", "github-step-json", "job-data.input.yml")]
    [InlineData("strict-json-with-multiline", "strict-json", "job-data-multiline.input.yml")]
    [InlineData("github-step-json-with-multiline", "github-step-json", "job-data-multiline.input.yml")]
    public async Task Success(string scenario, string outputOption, string dataOptionFilename)
    {
        const string artifactName = "job-data";
        const string artifactFilename = "job-data.json";
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("create-artifact-container.http-response.json"));
        });
        testHttpMessageHandler.MockUploadArtifactFileFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromFileContainerResourceUrl(
                    fileContainerResourceUrl: "https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982",
                    artifactName: artifactName,
                    artifactFilename: artifactFilename)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("finalize-artifact-container.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath(dataOptionFilename).ReadFile(),
            Output = outputOption,
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadErrorString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output)
            .AppendToMethodName("console-output")
            .UseParameters(scenario);
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> uploads the specified YML data as JSON as a workflow artifact.
    ///
    /// This test uses single line and multiline yaml data input to make sure both are handled as expected.
    /// Multiline yaml values should have the newline characters trimmed at the end.
    /// </summary>
    [Theory]
    [InlineData("job-data.input.yml")]
    [InlineData("job-data-multiline.input.yml")]
    public async Task UploadedArtifactIsIndentedJson(string dataOptionFilename)
    {
        const string artifactName = "job-data";
        const string artifactFilename = "job-data.json";
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("create-artifact-container.http-response.json"));
        });
        // not using the testHttpMessageHandler.MockUploadArtifactFileFromCurrentWorkflowRun auxiliary method because
        // I want to be able to capture the HttpRequest content that is being sent and the auxiliary methods I created on top of the
        // testHttpMessageHandler don't allow that.
        var artifactUploadContent = string.Empty;
        testHttpMessageHandler.MockHttpResponse(httpResponseMessageMockBuilder =>
        {
            const string fileContainerResourceUrl = "https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982";
            var responseContent = TestFiles.GetSharedFilepath("upload-artifact.http-response.json").ReadFileAsStringContent();
            httpResponseMessageMockBuilder
                .WhereRequestUriEquals($"{fileContainerResourceUrl}?itemPath={artifactName}%2F{artifactFilename}") // %2F is encoding for /
                .RespondWith(async (httpRequestMessage, cancellationToken) =>
                {
                    var requestBytes = await httpRequestMessage.Content!.ReadAsByteArrayAsync(cancellationToken);
                    artifactUploadContent = Encoding.UTF8.GetString(requestBytes);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        RequestMessage = httpRequestMessage,
                        Content = responseContent,
                    };
                });
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("finalize-artifact-container.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath(dataOptionFilename).ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);
        await Verify(artifactUploadContent)
            .AppendToMethodName("uploaded-artifact-content")
            .UseParameters(dataOptionFilename);
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> makes the expected HTTP requests when uploading a workflow artifact.
    /// </summary>
    [Fact]
    public async Task MakesExpectedHttpRequests()
    {
        const string artifactName = "job-data";
        const string artifactFilename = "job-data.json";
        var githubEnvironment = new TestsGitHubEnvironment();
        using var testHttpMessageHandler = new TestHttpMessageHandler();
        testHttpMessageHandler.MockCreateArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("create-artifact-container.http-response.json"));
        });
        testHttpMessageHandler.MockUploadArtifactFileFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromFileContainerResourceUrl(
                    fileContainerResourceUrl: "https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/2535982",
                    artifactName: artifactName,
                    artifactFilename: artifactFilename)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("finalize-artifact-container.http-response.json"));
        });
        var (httpClient, outboundHttpRequests) = TestHttpClient.CreateWithRecorder(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);
        await Verify(outboundHttpRequests).AppendToMethodName("outbound-http");
    }
}
