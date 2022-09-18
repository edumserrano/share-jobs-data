namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests check the validation on the options for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/>.
/// There tests are not for the Validators applied to the command options. They are for logic constrains enforced
/// before the command can be executed.
/// </summary>
[Trait("Category", XUnitCategories.Validation)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandValidationTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.AuthToken"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateAuthTokenOption(string authToken)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            AuthToken = authToken,
            Repo = "repo-name",
            RunId = "run-id",
        };
        var exception = await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("gitHubAuthToken cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.Repo"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateRepoOption(string repo)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = repo,
            RunId = "run-id",
        };
        var exception = await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("repository cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.RunId"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateRunIdOption(string runId)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = "repo-name",
            RunId = runId,
        };
        var exception = await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("runId cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactName"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactNameOption(string artifactName)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = "repo-name",
            RunId = "run-id",
            ArtifactName = artifactName,
        };
        var exception = await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactName cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactFilename> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactFilenameOption(string artifactFilename)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = Substitute.For<IGitHubEnvironment>();
        githubEnvironment.GitHubRepository.Returns("source-repo");
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            AuthToken = "auth-token",
            Repo = "repo-name",
            RunId = "run-id",
            ArtifactFilename = artifactFilename,
        };
        var exception = await Should.ThrowAsync<ArgumentException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactFilename cannot be null or whitespace.");
    }
}
