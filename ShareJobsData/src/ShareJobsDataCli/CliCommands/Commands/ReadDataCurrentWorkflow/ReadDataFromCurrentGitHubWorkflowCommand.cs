using ShareJobsDataCli.ArgumentValidations;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

[Command("read-data-current-workflow")]
public sealed class ReadDataCurrentWorkflow : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ReadDataCurrentWorkflow()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ReadDataCurrentWorkflow(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
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

    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            console.NotNull();
            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubCurrentWorkflowRunArticfactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var githubHttpClient = new GitHubCurrentWorkflowRunArticfactHttpClient(httpClient);
            var containerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
            var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);
            var sharedDataContent = await githubHttpClient.DownloadArtifactFileAsync(containerUrl, artifactContainerName, artifactFilePath);
            var jobDataJson = new JobDataJson(sharedDataContent);
            var jobDataKeysAndValues = jobDataJson.ToKeyValues();
            var stepOutput = new JobDataGitHubActionStepOutput(console);
            await stepOutput.WriteAsync(jobDataKeysAndValues);
        }
        catch (Exception e)
        {
            var message = @$"An error occurred trying to execute the command to read job data from the current workflow run.
Error:
- {e.Message}";
            throw new CommandException(message, innerException: e);
        }
    }
}
