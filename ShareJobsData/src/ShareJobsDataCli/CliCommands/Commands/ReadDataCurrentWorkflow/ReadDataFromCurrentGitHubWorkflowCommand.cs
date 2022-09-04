namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

[Command("read-data-current-workflow")]
public sealed class ReadDataFromCurrentGitHubWorkflowCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ReadDataFromCurrentGitHubWorkflowCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ReadDataFromCurrentGitHubWorkflowCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
    {
        _httpClient = httpClient.NotNull();
        _gitHubEnvironment = gitHubEnvironment;
    }

    [CommandOption(
        "artifact-name",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The name of the artifact.")]
    public string ArtifactName { get; init; } = CommandDefaults.ArtifactName;

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The filename that contains the data.")]
    public string ArtifactFilename { get; init; } = CommandDefaults.ArtifactFilename;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
        var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
        var repository = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
        var containerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);

        using var httpClient = _httpClient ?? GitHubCurrentWorkflowRunArticfactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
        var githubHttpClient = new GitHubCurrentWorkflowRunArticfactHttpClient(httpClient);
        var downloadResult = await githubHttpClient.DownloadArtifactFileAsync(containerUrl, artifactContainerName, artifactFilePath);
        if (!downloadResult.IsOk(out var gitHubArtifactItemContent, out var downloadError))
        {
            throw downloadError.ToCommandException();
        }

        var stepOutput = new JobDataGitHubActionStepOutput(console);
        await stepOutput.WriteAsync(gitHubArtifactItemContent);
    }
}
