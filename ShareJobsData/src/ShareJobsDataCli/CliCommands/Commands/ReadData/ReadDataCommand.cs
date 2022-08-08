namespace ShareJobsDataCli.CliCommands.Commands.ReadData;

[Command("read-data")]
public class ReadDataCommand : ICommand
{
    private readonly HttpClient? _httpClient;
    private readonly IGitHubEnvironment? _gitHubEnvironment;

    public ReadDataCommand()
    {
    }

    // used for test purposes to be able to mock external dependencies
    public ReadDataCommand(HttpClient httpClient, IGitHubEnvironment gitHubEnvironment)
    {
        _httpClient = httpClient.NotNull();
        _gitHubEnvironment = gitHubEnvironment;
    }

    //[CommandOption(
    //    "data",
    //    IsRequired = true,
    //    Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
    //    Description = "The data to share in YAML format.")]
    //public string DataAsYmlStr { get; init; } = default!;

    public async ValueTask ExecuteAsync(IConsole console)
    {
        try
        {
            console.NotNull();
            var githubEnvironment = _gitHubEnvironment ?? new GitHubEnvironment();
            var actionRuntimeToken = new GitHubActionRuntimeToken(githubEnvironment.GitHubActionRuntimeToken);
            var repository = new GitHubRepositoryName(githubEnvironment.GitHubRepository);
            using var httpClient = _httpClient ?? GitHubArticfactHttpClient.CreateHttpClient(actionRuntimeToken, repository);
            var githubHttpClient = new GitHubArticfactHttpClient(httpClient);
            var containerUrl = new GitHubArtifactContainerUrl(githubEnvironment.GitHubActionRuntimeUrl, githubEnvironment.GitHubActionRunId);
            var artifactContainerName = new GitHubArtifactContainerName("my-dotnet-artifact");
            var artifactFilePath = new GitHubArtifactItemFilePath(artifactContainerName, "shared-job-data.txt");
            var sharedDataContent = await githubHttpClient.DownloadArtifactFileAsync(containerUrl, artifactContainerName, artifactFilePath);
            await console.Output.WriteLineAsync(sharedDataContent);
            // TODO: also set the values as output for the step
        }
        catch (Exception e)
        {
            // TODO remove stacktrace from error message
            var message = @$"An error occurred trying to execute the command to parse the log from a Markdown link check step.
Error:
- {e.Message}";
            message += Environment.NewLine + e.StackTrace;
            throw new CommandException(message, innerException: e);
        }
    }
}
