namespace ShareJobsDataCli.Features.ReadDataCurrentWorkflow;

[Command(_commandName)]
public sealed class ReadDataFromCurrentGitHubWorkflowCommand : ICommand
{
    private const string _commandName = "read-data-current-workflow";
    private readonly HttpClient _httpClient;
    private readonly IGitHubEnvironment _gitHubEnvironment;

    // Default type activator is only capable of instantiating a type if it has a public parameterless constructor.
    // This ctor is used to avoid having to register the command in a more specific way as shown in:
    // https://github.com/Tyrrrz/CliFx#type-activation
    public ReadDataFromCurrentGitHubWorkflowCommand()
        : this(httpClient: null, gitHubEnvironment: null)
    {
    }

    // Input parameters are available for test purposes as they allow mocking external dependencies.
    public ReadDataFromCurrentGitHubWorkflowCommand(HttpClient? httpClient = default, IGitHubEnvironment? gitHubEnvironment = default)
    {
        _httpClient = httpClient ?? new HttpClient();
        _gitHubEnvironment = gitHubEnvironment ?? new GitHubEnvironment();
    }

    [CommandOption(
        "artifact-name",
        IsRequired = false,
        Validators = [typeof(NotNullOrWhitespaceOptionValidator)],
        Description = "The name of the artifact.")]
    public string ArtifactName { get; init; } = CommandOptionsDefaults.ArtifactName;

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = [typeof(NotNullOrWhitespaceOptionValidator)],
        Description = "The filename that contains the data.")]
    public string ArtifactFilename { get; init; } = CommandOptionsDefaults.ArtifactFilename;

    [CommandOption(
        "output",
        IsRequired = false,
        Validators = [typeof(NotNullOrWhitespaceOptionValidator)],
        Description = "How to output the job data in the step's output. It must be one of: strict-json, github-step-json.")]
    public string Output { get; init; } = "github-step-json";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var actionRuntimeToken = new GitHubActionRuntimeToken(_gitHubEnvironment.GitHubActionRuntimeToken);
        var repository = new GitHubRepositoryName(_gitHubEnvironment.GitHubRepository);
        var containerUrl = new GitHubArtifactContainerUrl(_gitHubEnvironment.GitHubActionRuntimeUrl, _gitHubEnvironment.GitHubActionRunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);
        var parseCommandOutputResult = ReadDataFromCurrentGitHubWorkflowCommandOutput.FromOption(console, Output);
        if (!parseCommandOutputResult.IsOk(out var commandOutput, out var parseCommandOutputError))
        {
            parseCommandOutputError.Throw(_commandName);
            return;
        }

        using var httpClient = _httpClient.ConfigureGitHubHttpClient(actionRuntimeToken, repository);
        var githubHttpClient = new GitHubDownloadArtifactFromCurrentWorkflowHttpClient(httpClient);
        var downloadResult = await githubHttpClient.DownloadArtifactFileAsync(
            containerUrl,
            artifactContainerName,
            artifactFilePath,
            CancellationToken.None);
        if (!downloadResult.IsOk(out var gitHubArtifactItemJsonContent, out var downloadError))
        {
            downloadError.Throw(_commandName);
            return;
        }

        var artifactItemAsJObject = gitHubArtifactItemJsonContent.AsJObject();
        var jobData = new JobData(artifactItemAsJObject);
        await commandOutput.WriteToConsoleAsync(jobData);
    }
}
