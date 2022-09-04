namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow;

[Command("read-data-different-workflow")]
public sealed class ReadDataFromDifferentGitHubWorkflowCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ReadDataFromDifferentGitHubWorkflowCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ReadDataFromDifferentGitHubWorkflowCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
    {
        _httpClient = httpClient.NotNull();
        _gitHubEnvironment = gitHubEnvironment;
    }

    [CommandOption(
        "auth-token",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "GitHub token used to download the job data artifact.")]
    public string AuthToken { get; init; } = default!;

    [CommandOption(
        "repo",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The repository for the workflow run in the format of {owner}/{repo}.")]
    public string Repo { get; init; } = default!;

    [CommandOption(
        "run-id",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The unique identifier of the workflow run that contains the job data artifact.")]
    public string RunId { get; init; } = default!;

    [CommandOption(
        "artifact-name",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string ArtifactName { get; init; } = CommandDefaults.ArtifactName;

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string ArtifactFilename { get; init; } = CommandDefaults.ArtifactFilename;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
        var sourceRepositoryName = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
        var authToken = new GitHubAuthToken(AuthToken);
        var jobDataArtifactRepositoryName = new GitHubRepositoryName(Repo);
        var runId = new GitHubRunId(RunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactItemFilename = new GitHubArtifactItemFilename(ArtifactFilename);

        using var httpClient = _httpClient ?? GitHubDifferentWorkflowRunArticfactHttpClient.CreateHttpClient(authToken, sourceRepositoryName);
        var githubHttpClient = new GitHubDifferentWorkflowRunArticfactHttpClient(httpClient);
        var downloadResult = await githubHttpClient.DownloadArtifactFileAsync(jobDataArtifactRepositoryName, runId, artifactContainerName, artifactItemFilename);
        if (!downloadResult.IsOk(out var gitHubArtifactItemContent, out var downloadError))
        {
            throw downloadError.ToCommandException();
        }

        var stepOutput = new JobDataGitHubActionStepOutput(console);
        await stepOutput.WriteAsync(gitHubArtifactItemContent);
    }
}
