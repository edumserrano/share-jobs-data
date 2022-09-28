namespace ShareJobsDataCli.Tests.CliCommands.ReadDataCurrentWorkflow;

/// <summary>
/// These tests check the default values for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.Defaults)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
public sealed class ReadDataFromCurrentGitHubWorkflowCommandOptionDefaultsTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactName"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactNameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromCurrentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactName.ShouldBe("job-data");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactFilename"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactFilenameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromCurrentGitHubWorkflowCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactFilename.ShouldBe("job-data.json");
    }
}
