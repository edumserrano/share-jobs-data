namespace ShareJobsDataCli.Tests.CliCommands.ReadDataDifferentWorkflow;

/// <summary>
/// These tests check the default values for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.Validation)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class ReadDataFromDifferentGitHubWorkflowCommandOptionDefaultsTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.AuthToken"/> command option default value.
    /// </summary>
    [Fact]
    public void AuthTokenDefaultsToNull()
    {
        var command = new ReadDataFromDifferentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.AuthToken.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.Repo"/> command option default value.
    /// </summary>
    [Fact]
    public void RepoDefaultsToNull()
    {
        var command = new ReadDataFromDifferentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.Repo.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.RunId"/> command option default value.
    /// </summary>
    [Fact]
    public void RunIdDefaultsToNull()
    {
        var command = new ReadDataFromDifferentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.RunId.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactName"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactNameDefaultsToNotNull()
    {
        var command = new ReadDataFromDifferentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.ArtifactName.ShouldBe("job-data");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand.ArtifactFilename"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactFilenameDefaultsToNotNull()
    {
        var command = new ReadDataFromDifferentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.ArtifactFilename.ShouldBe("job-data.json");
    }
}
