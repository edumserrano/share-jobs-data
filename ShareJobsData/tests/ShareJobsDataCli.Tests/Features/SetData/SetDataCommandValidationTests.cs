namespace ShareJobsDataCli.Tests.Features.SetData;

/// <summary>
/// These tests check the validation on the options for the <see cref="SetDataCommand"/>.
/// These tests are not for the Validators applied to the command options. They are for logic constrains enforced
/// before the command can be executed.
/// </summary>
[Trait("Category", XUnitCategories.Validation)]
[Trait("Category", XUnitCategories.SetDataCommand)]
public sealed class SetDataCommandValidationTests
{
    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.ArtifactName"/> command option.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactNameOption(string artifactName)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: githubEnvironment)
        {
            ArtifactName = artifactName,
        };

        var exception = await Should.ThrowAsync<GuardException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactName cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.ArtifactFilename"/> command option.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactFilenameOption(string artifactFilename)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: githubEnvironment)
        {
            ArtifactFilename = artifactFilename,
        };
        var exception = await Should.ThrowAsync<GuardException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactFilename cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="SetDataCommand.DataAsYmlStr"/> command option.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateDataAsYmlStrOption(string dataAsYmlStr)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = new TestsGitHubEnvironment();
        var command = new SetDataCommand(gitHubEnvironment: githubEnvironment)
        {
            DataAsYmlStr = dataAsYmlStr,
        };
        var exception = await Should.ThrowAsync<GuardException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("dataAsYmlStr cannot be null or whitespace.");
    }
}
