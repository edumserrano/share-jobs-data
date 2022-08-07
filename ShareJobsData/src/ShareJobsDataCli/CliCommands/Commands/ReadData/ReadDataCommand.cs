using System.Text.Json;

namespace ShareJobsDataCli.CliCommands.Commands.ReadData;

[Command("read-data")]
public class ReadDataCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ReadDataCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ReadDataCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
    {
        _httpClient = httpClient.NotNull();
        _gitHubEnvironment = gitHubEnvironment;
    }

    [CommandOption(
        "auth-token",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "GitHub token used to upload the artifact.")]
    public string AuthToken { get; init; } = default!;

    //[CommandOption(
    //    "data",
    //    IsRequired = true,
    //    Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
    //    Description = "The data to share in YAML format.")]
    //public string DataAsYmlStr { get; init; } = default!;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            console.NotNull();
            var authToken = new GitHubAuthToken(AuthToken);

            //var raw = "{ \"count\":2,\"value\":[{ \"containerId\":1532216,\"scopeIdentifier\":\"00000000-0000-0000-0000-000000000000\",\"path\":\"my-dotnet-artifact\",\"itemType\":\"folder\",\"status\":\"created\",\"dateCreated\":\"2022-08-07T15:31:22.69Z\",\"dateLastModified\":\"2022-08-07T15:31:22.69Z\",\"createdBy\":\"6e9b9734-3d2f-4573-b4ae-ad0d11728d76\",\"lastModifiedBy\":\"6e9b9734-3d2f-4573-b4ae-ad0d11728d76\",\"itemLocation\":\"https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/1532216?itemPath=my-dotnet-artifact&metadata=True\",\"contentLocation\":\"https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/1532216?itemPath=my-dotnet-artifact\",\"contentId\":\"\"},{ \"containerId\":1532216,\"scopeIdentifier\":\"00000000-0000-0000-0000-000000000000\",\"path\":\"my-dotnet-artifact/shared-job-data.txt\",\"itemType\":\"file\",\"status\":\"created\",\"fileLength\":312,\"fileEncoding\":1,\"fileType\":1,\"dateCreated\":\"2022-08-07T15:31:22.69Z\",\"dateLastModified\":\"2022-08-07T15:31:22.73Z\",\"createdBy\":\"6e9b9734-3d2f-4573-b4ae-ad0d11728d76\",\"lastModifiedBy\":\"6e9b9734-3d2f-4573-b4ae-ad0d11728d76\",\"itemLocation\":\"https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/1532216?itemPath=my-dotnet-artifact%2Fshared-job-data.txt&metadata=True\",\"contentLocation\":\"https://pipelines.actions.githubusercontent.com/pasYWZMKAGeorzjszgve9v6gJE03WMQ2NXKn6YXBa7i57yJ5WP/_apis/resources/Containers/1532216?itemPath=my-dotnet-artifact%2Fshared-job-data.txt\",\"fileId\":2139,\"contentId\":\"\"}]}";
            //var options = new JsonSerializerOptions()
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //};
            //var model = System.Text.Json.JsonSerializer.Deserialize<GitHubListArtifactsResponse>(raw, options);
            //var model = System.Text.Json.JsonSerializer.Deserialize<GitHubGetContainerItemsResponse>(raw, options);


            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepository(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var githubHttpClient = new GitHubHttpClient(httpClient);
            var containerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            await githubHttpClient.DownloadArtifactsAsync(containerUrl, "my-dotnet-artifact");
            //await githubHttpClient.DownloadArtifactAsync(repository, artifact);
            // TODO: also set the values as output for the step


            //var repo = new GitHubRepository(Repo);
            //var runId = new GitHubRunId(RunId);
            //var jobName = new GitHubJobName(JobName);
            //var stepName = new GitHubStepName(StepName);
            //var outputOptions = new OutputOptions(OutputOptions);
            //var outputJsonFilePath = new OutputJsonFilepathOption(OutputJsonFilepath);
            //var outputMarkdownFilePath = new OutputMarkdownFilepathOption(OutputMarkdownFilepath);
            //var outputFormats = OutputFormats.Create(outputOptions, _file, console, outputJsonFilePath, outputMarkdownFilePath);

            //using var httpClient = _httpClient ?? GitHubHttpClient.Create(authToken);
            //var gitHubHttpClient = new GitHubHttpClient(httpClient);
            //var gitHubWorkflowRunLogs = new GitHubWorkflowRunLogs(gitHubHttpClient);
            //var stepLog = await gitHubWorkflowRunLogs.GetStepLogAsync(repo, runId, jobName, stepName);
            //var output = MarkdownLinkCheckOutputParser.Parse(stepLog, CaptureErrorsOnly);
            //foreach (var outputFormat in outputFormats)
            //{
            //    await outputFormat.WriteAsync(output);
            //}
        }
        catch (Exception e)
        {
            var message = @$"An error occurred trying to execute the command to parse the log from a Markdown link check step.
Error:
- {e.Message}";
            message += Environment.NewLine + e.StackTrace;
            throw new CommandException(message, innerException: e);
        }
    }
}
