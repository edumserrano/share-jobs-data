namespace ShareJobsDataCli.Tests.Features.ReadDataDifferentWorkflow;

/// <summary>
/// These tests check the default values for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.Defaults)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
public sealed class ReadDataFromDifferentGitHubWorkflowCommandOptionDefaultsTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.AuthToken"/> command option default value.
    /// </summary>
    [Fact]
    public void AuthTokenDefaultsToNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.AuthToken.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.Repo"/> command option default value.
    /// </summary>
    [Fact]
    public void RepoDefaultsToNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.Repo.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.RunId"/> command option default value.
    /// </summary>
    [Fact]
    public void RunIdDefaultsToNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.RunId.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactName"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactNameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactName.ShouldBe("job-data");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactFilename"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactFilenameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactFilename.ShouldBe("job-data.json");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.Output"/> command option default value.
    /// </summary>
    [Fact]
    public void OutputDefaultsToGitHubStepJson()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromDifferentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.Output.ShouldBe("github-step-json");
    }
}
