namespace ShareJobsDataCli.Tests.CliCommands.SetData.DependencyErrors;

/// <summary>
/// These tests check what happens when the upload artifact file HTTP dependency of the <see cref="SetDataCommand"/> fails.
/// </summary>
[Trait("Category", XUnitCategories.DependencyFailure)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public class FailedHttpToFinalizeArtifactContainerTests
{
    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the HTTP request to
    /// finalize the artifact container fails.
    /// Simulating an HttpStatusCode.InternalServerError from the finalize artifact container response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCode()
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
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact-file.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError);
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the HTTP request to
    /// finalize the artifact container fails.
    /// Simulating an HttpStatusCode.InternalServerError from the finalize artifact container response.
    /// </summary>
    [Fact]
    public async Task ErrorHttpStatusCodeWithBody()
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
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact-file.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.InternalServerError)
                .WithResponseContent("Oops, something went wrong.");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> shows expected error message when the HTTP request to
    /// finalize the artifact container fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that results in null from the finalize artifact container response.
    /// This allows testing the formatting of the error message on the output when the JSON deserialization results in a null value.
    /// </summary>
    [Fact]
    public async Task NullDeserialization()
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
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact-file.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContent("null");
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }

    /// <summary>
    /// Tests that the <see cref="SetDataCommand"/> hows expected error when the HTTP request to
    /// finalize the artifact container fails.
    /// Simulating an HttpStatusCode.OK and a JSON deserialization that fails validation from the finalize artifact container response.
    /// This allows testing the formatting of the error message on the output when the validation on the deserialized JSON model fails.
    /// </summary>
    [Fact]
    public async Task JsonModelValidation()
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
                .WithResponseContentFromFilepath(TestFiles.GetSharedFilepath("upload-artifact-file.http-response.json"));
        });
        testHttpMessageHandler.MockFinalizeArtifactContainerFromCurrentWorkflowRun(builder =>
        {
            builder
                .FromCurrentWorkflowRun(
                    githubEnvironment.GitHubActionRuntimeUrl,
                    githubEnvironment.GitHubActionRunId,
                    containerName: artifactName)
                .WithResponseStatusCode(HttpStatusCode.OK)
                .WithResponseContentFromFilepath(TestFiles.GetFilepath("finalize-artifact-container.http-response.json"));
        });
        using var httpClient = new HttpClient(testHttpMessageHandler);

        var command = new SetDataCommand(httpClient, githubEnvironment)
        {
            ArtifactName = artifactName,
            ArtifactFilename = artifactFilename,
            DataAsYmlStr = TestFiles.GetSharedFilepath("job-data.input.yml").ReadFile(),
        };
        using var console = new FakeInMemoryConsole();
        await command.ExecuteAsync(console);

        console.ReadOutputString().ShouldBeEmpty();
        var output = console.ReadAllAsString();
        await Verify(output).AppendToMethodName("console-output");
    }
}
