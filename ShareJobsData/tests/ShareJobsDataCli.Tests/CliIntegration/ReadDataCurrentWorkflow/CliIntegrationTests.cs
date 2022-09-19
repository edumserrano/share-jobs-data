namespace ShareJobsDataCli.Tests.CliIntegration.ReadDataCurrentWorkflow;

/// <summary>
/// These tests make sure that the CLI interface for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> command is as expected.
/// IE: if the command name changes or the options change then these tests would pick that up.
/// These tests also test the <see cref="IBindingValidator"/> validators of the command options.
/// </summary>
[Trait("Category", XUnitCategories.CliIntegration)]
[Trait("Category", XUnitCategories.ReadDataFromCurrentGitHubWorkflowCommand)]
[UsesVerify]
public class CliIntegrationTests
{
    /// <summary>
    /// Tests the validation of the --artifact-name option for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("empty-string", "")]
    [InlineData("white-space", "   ")]
    public async Task AuthTokenValidation(string scenario, string artifactName)
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-current-workflow",
            "--artifact-name", artifactName,
            "--data-filename", "some filename",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings).UseParameters(scenario);
    }

    /// <summary>
    /// Tests the validation of the --repo option for the <see cref="ReadDataFromCurrentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("empty-string", "")]
    [InlineData("white-space", "   ")]
    public async Task RepoValidation(string scenario, string artifactFilename)
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-current-workflow",
            "--artifact-name", "some artifact name",
            "--data-filename", artifactFilename,
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings).UseParameters(scenario);
    }
}
