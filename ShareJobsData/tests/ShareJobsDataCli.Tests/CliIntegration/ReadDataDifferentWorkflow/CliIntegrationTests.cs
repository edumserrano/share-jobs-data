namespace ShareJobsDataCli.Tests.CliIntegration.ReadDataDifferentWorkflow;

/// <summary>
/// These tests make sure that the CLI interface for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command is as expected.
/// IE: if the command name changes or the options change then these tests would pick that up.
/// These tests also test the <see cref="IBindingValidator"/> validators of the command options.
/// </summary>
[Trait("Category", XUnitCategories.CliIntegration)]
[Trait("Category", XUnitCategories.ReadDataFromDifferentGitHubWorkflowCommand)]
[UsesVerify]
public sealed class CliIntegrationTests
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
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
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
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
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
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
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
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --auth-token option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task AuthTokenValidation(string authToken)
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", authToken,
            "--repo", "some repo",
            "--run-id", "some run id",
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --repo option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RepoValidation(string repo)
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--repo", repo,
            "--run-id", "some run id",
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --run-id option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task RunIdValidation(string runId)
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "read-data-different-workflow",
            "--auth-token", "some auth token",
            "--repo", "some repo",
            "--run-id", runId,
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --artifact-name option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ArtifactNameValidation(string artifactName)
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
            "--artifact-name", artifactName,
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --data-filename option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ArtifactFilenameValidation(string artifactFilename)
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
            "--data-filename", artifactFilename,
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --output option for the <see cref="ReadDataFromDifferentGitHubWorkflowCommand"/> command.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task OutputValidation(string outputOption)
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
            "--data-filename", "some data filename",
            "--output", outputOption,
        };
        await app.RunAsync(args);

        var output = console.ReadAllAsString();
        await Verify(output)
            .ScrubAppName()
            .AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }
}
