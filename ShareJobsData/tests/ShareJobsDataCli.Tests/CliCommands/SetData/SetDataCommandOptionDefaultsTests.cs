namespace ShareJobsDataCli.Tests.CliCommands.SetData;

/// <summary>
/// These tests check the default values for the <see cref="SetDataCommand"/>.
/// </summary>
[Trait("Category", XUnitCategories.Defaults)]
[Trait("Category", XUnitCategories.SetDataCommand)]
public sealed class SetDataCommandOptionDefaultsTests
{
    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.ArtifactName"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactNameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactName.ShouldBe("job-data");
    }

    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.ArtifactFilename"/> command option default value.
    /// </summary>
    [Fact]
    public void ArtifactFilenameDefaultsToNotNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.ArtifactFilename.ShouldBe("job-data.json");
    }

    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.DataAsYmlStr"/> command option default value.
    /// </summary>
    [Fact]
    public void DataAsYmlStrDefaultsToNull()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.DataAsYmlStr.ShouldBeNull();
    }

    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.Output"/> command option default value.
    /// </summary>
    [Fact]
    public void OutputDefaultsToNone()
    {
        var testsGitHubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: testsGitHubEnvironment);
        using var console = new FakeInMemoryConsole();
        command.Output.ShouldBe("none");
    }
}
