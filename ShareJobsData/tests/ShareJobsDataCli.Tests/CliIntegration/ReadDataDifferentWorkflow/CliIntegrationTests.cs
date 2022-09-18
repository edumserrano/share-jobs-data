namespace ShareJobsDataCli.Tests.CliIntegration.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the CLI interface is as expected.
/// IE: if the command name changes or the options change then these tests would pick that up.
/// These tests also test the <see cref="IBindingValidator"/> validators of the command options.
/// </summary>
[Trait("Category", XUnitCategories.CliIntegration)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public class CliIntegrationTests
{
    /// <summary>
    /// Tests that if no arguments are passed the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command,
    /// the CLI  returns the help text for the command.
    /// </summary>
    [Fact]
    public async Task NoArguments()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        await app.RunAsync("read-data-different-workflow");
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }

    /// <summary>
    /// Tests that the --auth-token option is required for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Fact]
    public async Task AuthTokenOptionIsRequired()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--repo", "some repo",
            "--run-id", "some run id",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();
        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }

    /// <summary>
    /// Tests that the --repo option is required for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Fact]
    public async Task RepoOptionIsRequired()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--run-id", "some run id",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }

    /// <summary>
    /// Tests that the --run-id option is required for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Fact]
    public async Task RunIdOptionIsRequired()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--repo", "some repo",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }

    /// <summary>
    /// Tests that the --artifact-name option is must have a value for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Fact]
    public async Task ArtifactNameMustHaveValue()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--repo", "some repo",
            "--run-id", "some run id",
            "--artifact-name", "  ",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }

    /// <summary>
    /// Tests that the --data-filename option is must have a value for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Fact]
    public async Task ArtifactFilenameMustHaveValue()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--repo", "some repo",
            "--run-id", "some run id",
            "--artifact-name", "some artifact name",
            "--data-filename", "  ",
        };
        await app.RunAsync(args);
        var output = console.ReadAllAsString();

        var settings = new VerifySettings();
        settings.ScrubAppName();
        await Verify(output, settings);
    }
}
