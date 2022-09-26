namespace ShareJobsDataCli.CliCommands.Commands.SetData;

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
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The name of the artifact.")]
    public string ArtifactName { get; init; } = CommandDefaults.ArtifactName;

    [CommandOption(
        "data-filename",
        IsRequired = false,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The filename that contains the data.")]
    public string ArtifactFilename { get; init; } = CommandDefaults.ArtifactFilename;

    [CommandOption(
        "data",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "The data to share in YAML format.")]
    public string DataAsYmlStr { get; init; } = default!;

    [CommandOption(
        "set-step-output",
        IsRequired = false,
        Description = "Whether or not the job data should also be set as a step output.")]
    public bool SetStepOutput { get; init; } = true;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        console.NotNull();

        var actionRuntimeToken = new GitHubActionRuntimeToken(_gitHubEnvironment.GitHubActionRuntimeToken);
        var repository = new GitHubRepositoryName(_gitHubEnvironment.GitHubRepository);
        var artifactContainerUrl = new GitHubArtifactContainerUrl(_gitHubEnvironment.GitHubActionRuntimeUrl, _gitHubEnvironment.GitHubActionRunId);
        var artifactContainerName = new GitHubArtifactContainerName(ArtifactName);
        var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, ArtifactFilename);
        var createJobDataAsJsonResult = JobDataAsJson.FromYml(DataAsYmlStr);
        if (!createJobDataAsJsonResult.IsOk(out var jobDataAsJson, out var createJobDataAsJsonError))
        {
            await createJobDataAsJsonError.WriteToConsoleAsync(console, _commandName);
            return;
        }

        var artifactFileUploadRequest = new GitHubArtifactFileUploadRequest(artifactFilePath, fileUploadContent: jobDataAsJson.AsJson());
        using var httpClient = _httpClient.ConfigureGitHubCurrentWorkflowRunArticfactHttpClient(actionRuntimeToken, repository);
        var githubHttpClient = new GitHubCurrentWorkflowRunArticfactHttpClient(httpClient);
        var uploadArtifact = await githubHttpClient.UploadArtifactFileAsync(artifactContainerUrl, artifactContainerName, artifactFileUploadRequest);
        if (!uploadArtifact.IsOk(out var _, out var uploadError))
        {
            await uploadError.WriteToConsoleAsync(console, _commandName);
            return;
        }

        if (SetStepOutput)
        {
            var stepOutput = new JobDataGitHubActionStepOutput(console);
            await stepOutput.WriteAsync(jobDataAsJson);
        }
    }
}
