namespace ShareJobsDataCli.Tests.CliIntegration.SetData;

/// <summary>
/// These tests make sure that the CLI interface for the <see cref="SetDataCommand"/> command is as expected.
/// IE: if the command name changes or the options change then these tests would pick that up.
/// These tests also test the <see cref="IBindingValidator"/> validators of the command options.
/// </summary>
[Trait("Category", XUnitCategories.CliIntegration)]
[Trait("Category", XUnitCategories.SetDataCommand)]
[UsesVerify]
public sealed class CliIntegrationTests
{
    /// <summary>
    /// Tests that if no arguments are passed the <see cref="SetDataCommand"/> command,
    /// the CLI  returns the help text for the command.
    /// </summary>
    [Fact]
    public async Task NoArguments()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        await app.RunAsync("set-data");

        var settings = new VerifySettings();
        settings.ScrubAppName();
        var output = console.ReadAllAsString();
        await Verify(output, settings).AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests that the --data option is required for the <see cref="SetDataCommand"/> command.
    /// </summary>
    [Fact]
    public async Task DataOptionIsRequired()
    {
        using var console = new FakeInMemoryConsole();
        var app = new ShareDataBetweenJobsCli();
        app.CliApplicationBuilder.UseConsole(console);
        var args = new[]
        {
            "set-data",
        };
        await app.RunAsync(args);

        var settings = new VerifySettings();
        settings.ScrubAppName();
        var output = console.ReadAllAsString();
        await Verify(output, settings).AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --artifact-name option for the <see cref="SetDataCommand"/> command.
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
            "set-data",
            "--data", "some data",
            "--artifact-name", artifactName,
        };
        await app.RunAsync(args);

        var settings = new VerifySettings();
        settings.ScrubAppName();
        var output = console.ReadAllAsString();
        await Verify(output, settings).AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }

    /// <summary>
    /// Tests the validation of the --data-filename option for the <see cref="SetDataCommand"/> command.
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
            "set-data",
            "--data", "some data",
            "--data-filename", artifactFilename,
        };
        await app.RunAsync(args);

        var settings = new VerifySettings();
        settings.ScrubAppName();
        var output = console.ReadAllAsString();
        await Verify(output, settings).AppendToMethodName("console-output");
        console.ReadOutputString().ShouldNotBeEmpty();
        console.ReadErrorString().ShouldNotBeEmpty();
    }
}
