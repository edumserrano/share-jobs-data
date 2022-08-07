namespace ShareJobsDataCli.CliCommands.Commands.ShareData;

[Command("share-data")]
public class ShareDataCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ShareDataCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ShareDataCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
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

    [CommandOption(
        "data",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string DataAsYmlStr { get; init; } = default!;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            console.NotNull();
            var authToken = new GitHubAuthToken(AuthToken);

            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            var dataAsYml = deserializer.Deserialize<object>(DataAsYmlStr);
            var dataAsJson = JsonConvert.SerializeObject(dataAsYml, Formatting.Indented);

            await console.Output.WriteLineAsync(dataAsJson);

            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepository(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubUploadArtifactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var containerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            var artifact = new GitHubUploadArtifact("my-dotnet-artifact", "shared-job-data.txt", dataAsJson);
            var githubHttpClient = new GitHubUploadArtifactHttpClient(httpClient);
            await githubHttpClient.UploadArtifactAsync(containerUrl, artifact);
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
            throw new CommandException(message, innerException: e);
        }
    }
}

public record GitHubUpdateArtifactResponse2
(
    //int ContainerId,
    string ScopeIdentifier,
    string Path,
    string ItemType,
    string Status
//int FileLength,
//int FileEncoding,
//int FileType,
//DateTime DateCreated,
//DateTime DateLastModified,
//string CreatedBy,
//string LastModifiedBy,
//int FileId,
//string ContentId
);
