namespace ShareJobsDataCli.Tests.CliCommands.ReadDataCurrentWorkflow;

/// <summary>
/// These tests check the default values for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.Validation)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
public class ReadDataFromCurrentGitHubWorkflowCommandOptionDefaultsTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactName"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactNameDefaultsToNotNull()
    {
        var command = new ReadDataFromCurrentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.ArtifactName.ShouldBe("job-data");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactFilename"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactFilenameDefaultsToNotNull()
    {
        var command = new ReadDataFromCurrentGitHubWorkflowCommand();
        using var console = new FakeInMemoryConsole();
        command.ArtifactFilename.ShouldBe("job-data.json");
    }
}
