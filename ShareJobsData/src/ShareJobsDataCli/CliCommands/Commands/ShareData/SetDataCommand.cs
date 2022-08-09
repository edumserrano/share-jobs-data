namespace ShareJobsDataCli.CliCommands.Commands.ShareData;

[Command("set-data")]
public class SetDataCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public SetDataCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public SetDataCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
    {
        _httpClient = httpClient.NotNull();
        _gitHubEnvironment = gitHubEnvironment;
    }

    [CommandOption(
        "artifact-name",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The name of the artifact.")]
    public string ArtifactName { get; init; } = "job-data";

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The filename that contains the data.")]
    public string ArtifactFilename { get; init; } = "job-data.json";

    [CommandOption(
        "data",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string DataAsYmlStr { get; init; } = default!;

    [CommandOption(
        "set-step-output",
        Description = "Whether or not the job data should also be set as a step output.")]
    public bool SetStepOutput { get; init; }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            console.NotNull();
            var jobDataYml = new JobDataYml(DataAsYmlStr);
            var jobDataJson = jobDataYml.ToJson();
            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubCurrentWorkflowRunArticfactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var artifactContainerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
            var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);
            var artifactFileUploadRequest = new GitHubArtifactFileUploadRequest(artifactFilePath, jobDataJson);
            var githubHttpClient = new GitHubCurrentWorkflowRunArticfactHttpClient(httpClient);
            await githubHttpClient.UploadArtifactFileAsync(artifactContainerUrl, artifactContainerName, artifactFileUploadRequest);
            if (SetStepOutput)
            {
                var jobDataKeysAndValues = jobDataJson.ToKeyValues();
                var stepOutput = new JobDataGitHubActionStepOutput(console);
                await stepOutput.WriteAsync(jobDataKeysAndValues);
            }
        }
        catch (Exception e)
        {
            var message = @$"An error occurred trying to execute the command to set job data.
Error:
- {e.Message}";
            throw new CommandException(message, innerException: e);
        }
    }
}
