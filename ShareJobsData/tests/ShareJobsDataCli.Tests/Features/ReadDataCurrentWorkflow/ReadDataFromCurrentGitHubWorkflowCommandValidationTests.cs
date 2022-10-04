namespace ShareJobsDataCli.Tests.Features.ReadDataCurrentWorkflow;

/// <summary>
/// These tests check the validation on the options for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/>.
/// These tests are not for the Validators applied to the command options. They are for logic constrains enforced
/// before the command can be executed.
/// </summary>
[Trait("Category", XUnitCategories.Validation)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
public sealed class ReadDataFromCurrentGitHubWorkflowCommandValidationTests
{
    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactName"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactNameOption(string artifactName)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromCurrentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            ArtifactName = artifactName,
        };
        var exception = await Should.ThrowAsync<GuardException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactName cannot be null or whitespace.");
    }

    /// <summary>
    /// Validation test for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand.ArtifactFilename"/> command option.
    /// </summary>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ValidateArtifactFilenameOption(string artifactFilename)
    {
        using var console = new FakeInMemoryConsole();
        var githubEnvironment = new TestsGitHubEnvironment();
        var command = new ReadDataFromCurrentGitHubWorkflowCommand(gitHubEnvironment: githubEnvironment)
        {
            ArtifactFilename = artifactFilename,
        };
        var exception = await Should.ThrowAsync<GuardException>(() => command.ExecuteAsync(console).AsTask());
        exception.Message.ShouldBe("artifactFilename cannot be null or whitespace.");
    }
}
