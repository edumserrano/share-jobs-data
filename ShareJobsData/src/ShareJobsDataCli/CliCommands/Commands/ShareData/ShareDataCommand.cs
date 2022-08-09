using ShareJobsDataCli.GitHub.Artifact.SameWorkflowRun;

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
            var jobDataYml = new JobDataYml(DataAsYmlStr);
            var jobDataJson = jobDataYml.ToJson();
            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubSameWorkflowRunArticfactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var artifactContainerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            var artifactContainerName = new GitHubArtifactContainerName("my-dotnet-artifact");
            var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, "shared-job-data.txt");
            var artifactFileUploadRequest = new GitHubArtifactFileUploadRequest(artifactFilePath, jobDataJson);
            var githubHttpClient = new GitHubSameWorkflowRunArticfactHttpClient(httpClient);
            await githubHttpClient.UploadArtifactFileAsync(artifactContainerUrl, artifactContainerName, artifactFileUploadRequest);
            // TODO: also set the values as output for the step
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
