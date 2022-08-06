using ShareJobsDataCli.GitHub;

namespace ShareJobsDataCli.CliCommands.ShareData;

[Command("share-data")]
public class ShareAsWorkflowArtifactCommand : ICommand
{
    [CommandOption(
        "auth-token",
        IsRequired = true,
        Validators = new Type[] { typeof(NotNullOrWhitespaceOptionValidator) },
        Description = "GitHub token used to upload the artifact.")]
    public string AuthToken { get; init; } = default!;

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
            var authToken = new GitHubAuthToken(AuthToken);

            var deserializer = new DeserializerBuilder()
                .IgnoreUnmatchedProperties()
                .Build();
            var dataAsYml = deserializer.Deserialize<object>(DataAsYmlStr);
            var dataAsJson = JsonConvert.SerializeObject(dataAsYml, Formatting.Indented);

            await console.Output.WriteLineAsync(dataAsJson);
            await console.Output.WriteLineAsync($"ACTIONS_RUNTIME_URL={Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL")}");

            var githubHttpClient = new GitHubHttpClient();
            await githubHttpClient.UploadArtifactAsync(dataAsJson);

            //var repo = new GitHubRepository(Repo);
            //var runId = new GitHubRunId(RunId);
            //var jobName = new GitHubJobName(JobName);
            //var stepName = new GitHubStepName(StepName);
            //var outputOptions = new OutputOptions(OutputOptions);
            //var outputJsonFilePath = new OutputJsonFilepathOption(OutputJsonFilepath);
            //var outputMarkdownFilePath = new OutputMarkdownFilepathOption(OutputMarkdownFilepath);
            //var outputFormats = OutputFormats.Create(outputOptions, _file, console, outputJsonFilePath, outputMarkdownFilePath);

            //using var httpClient = _httpClient ?? GitHubHttpClient.Create(authToken);
            //var gitHubHttpClient = new GitHubHttpClient(httpClient);
            //var gitHubWorkflowRunLogs = new GitHubWorkflowRunLogs(gitHubHttpClient);
            //var stepLog = await gitHubWorkflowRunLogs.GetStepLogAsync(repo, runId, jobName, stepName);
            //var output = MarkdownLinkCheckOutputParser.Parse(stepLog, CaptureErrorsOnly);
            //foreach (var outputFormat in outputFormats)
            //{
            //    await outputFormat.WriteAsync(output);
            //}
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
