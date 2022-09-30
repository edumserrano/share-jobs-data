namespace ShareJobsDataCli.CliCommands.Commands.ReadDataDifferentWorkflow;

[Command(_commandName)]
public sealed class ReadDataFromDifferentGitHubWorkflowCommand : ICommand
{
    private const string _commandName = "read-data-different-workflow";
    private readonly HttpClient _httpClient;
    private readonly IGitHubEnvironment _gitHubEnvironment;

    // Default type activator is only capable of instantiating a type if it has a public parameterless constructor.
    // This ctor is used to avoid having to register the command in a more specific way as shown in:
    // https://github.com/Tyrrrz/CliFx#type-activation
    public ReadDataFromDifferentGitHubWorkflowCommand()
        : this(httpClient: null, gitHubEnvironment: null)
    {
    }

    // Input parameters are available for test purposes as they allow mocking external dependencies.
    public ReadDataFromDifferentGitHubWorkflowCommand(HttpClient? httpClient = default, IGitHubEnvironment? gitHubEnvironment = default)
    {
        _httpClient = httpClient ?? new HttpClient();
        _gitHubEnvironment = gitHubEnvironment ?? new GitHubEnvironment();
    }

    [CommandOption(
        "auth-token",
        IsRequired = true,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "GitHub token used to download the job data artifact.")]
    public string AuthToken { get; init; } = default!;

    [CommandOption(
        "repo",
        IsRequired = true,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The repository for the workflow run in the format of {owner}/{repo}.")]
    public string Repo { get; init; } = default!;

    [CommandOption(
        "run-id",
        IsRequired = true,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The unique identifier of the workflow run that contains the job data artifact.")]
    public string RunId { get; init; } = default!;

    [CommandOption(
        "artifact-name",
        IsRequired = false,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string ArtifactName { get; init; } = CommandOptionsDefaults.ArtifactName;

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string ArtifactFilename { get; init; } = CommandOptionsDefaults.ArtifactFilename;

    [CommandOption(
        "output",
        IsRequired = false,
        Validators = new[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "How to output the job data in the step's output. It must be one of: strict-json, github-step-json.")]
    public string Output { get; init; } = "github-step-json";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var sourceRepositoryName = new GitHubRepositoryName(_gitHubEnvironment.GitHubRepository);
        var authToken = new GitHubAuthToken(AuthToken);
        var jobDataArtifactRepositoryName = new GitHubRepositoryName(Repo);
        var runId = new GitHubRunId(RunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactItemFilename = new GitHubArtifactItemFilename(ArtifactFilename);
        var parseCommandOutputResult = ReadDataFromDifferentGitHubWorkflowCommandOutput.FromOption(console, Output);
        if (!parseCommandOutputResult.IsOk(out var commandOutput, out var parseCommandOutputError))
        {
            await parseCommandOutputError.WriteToConsoleAsync(console, _commandName);
            return;
        }

        using var httpClient = _httpClient.ConfigureGitHubHttpClient(authToken, sourceRepositoryName);
        var githubHttpClient = new GitHubDownloadArtifactFromDifferentWorkflowHttpClient(httpClient);
        var downloadResult = await githubHttpClient.DownloadArtifactFileAsync(jobDataArtifactRepositoryName, runId, artifactContainerName, artifactItemFilename);
        if (!downloadResult.IsOk(out var gitHubArtifactItemJsonContent, out var downloadError))
        {
            await downloadError.WriteToConsoleAsync(console, _commandName);
            return;
        }

        var artifactItemAsJObject = gitHubArtifactItemJsonContent.AsJObject();
        var jobData = new JobData(artifactItemAsJObject);
        await commandOutput.WriteToConsoleAsync(jobData);
    }
}
