namespace ShareJobsDataCli.Features.SetData;

[Command(_commandName)]
public sealed class SetDataCommand : ICommand
{
    private const string _commandName = "set-data";
    private readonly HttpClient _httpClient;
    private readonly IGitHubEnvironment _gitHubEnvironment;

    // Default type activator is only capable of instantiating a type if it has a public parameterless constructor.
    // This ctor is used to avoid having to register the command in a more specific way as shown in:
    // https://github.com/Tyrrrz/CliFx#type-activation
    public SetDataCommand()
        : this(httpClient: null, gitHubEnvironment: null)
    {
    }

    // Input parameters are available for test purposes as they allow mocking external dependencies.
    public SetDataCommand(HttpClient? httpClient = default, IGitHubEnvironment? gitHubEnvironment = default)
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
        "data",
        IsRequired = true,
        Validators = [typeof(NotNullOrWhitespaceOptionValidator)],
        Description = "The data to share in YAML format.")]
    public string DataAsYmlStr { get; init; } = default!;

    [CommandOption(
        "output",
        IsRequired = false,
        Validators = [typeof(NotNullOrWhitespaceOptionValidator)],
        Description = "How to output the job data in the step's output. It must be one of: none, strict-json, github-step-json.")]
    public string Output { get; init; } = "none";

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var actionRuntimeToken = new GitHubActionRuntimeToken(_gitHubEnvironment.GitHubActionRuntimeToken);
        var repository = new GitHubRepositoryName(_gitHubEnvironment.GitHubRepository);
        var artifactContainerUrl = new GitHubArtifactContainerUrl(_gitHubEnvironment.GitHubActionRuntimeUrl, _gitHubEnvironment.GitHubActionRunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);
        var parseCommandOutputResult = SetDataCommandOutput.FromOption(console, Output);
        if (!parseCommandOutputResult.IsOk(out var commandOutput, out var parseCommandOutputError))
        {
            parseCommandOutputError.Throw(_commandName);
            return;
        }

        var createJobDataResult = JobData.FromYml(DataAsYmlStr);
        if (!createJobDataResult.IsOk(out var jobData, out var createJobDataError))
        {
            createJobDataError.Throw(_commandName);
            return;
        }

        var artifactFileUploadRequest = new GitHubArtifactFileUploadRequest(artifactFilePath, fileUploadContent: jobData.AsJson());
        using var httpClient = _httpClient.ConfigureGitHubHttpClient(actionRuntimeToken, repository);
        var githubHttpClient = new GitHubUploadArtifactHttpClient(httpClient);
        var uploadArtifact = await githubHttpClient.UploadArtifactFileAsync(artifactContainerUrl, artifactContainerName, artifactFileUploadRequest);
        if (!uploadArtifact.IsOk(out _, out var uploadError))
        {
            uploadError.Throw(_commandName);
            return;
        }

        await commandOutput.WriteToConsoleAsync(jobData);
    }
}
